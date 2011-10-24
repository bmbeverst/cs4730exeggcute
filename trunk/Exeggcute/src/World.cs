using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.gui;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.input;
using Exeggcute.src.contexts;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;

namespace Exeggcute.src
{
    /// <summary>
    /// Stores and processes the current contexts in game.
    /// Example: The settings menu is pushed on top of the main menu,
    /// or the ReallyQuit? menu is pushed on top of the PauseMenu is 
    /// pushed on the Level Context.
    /// </summary>
    static class World 
    {
        private static Stack<IContext> stack = new Stack<IContext>();
        private static bool isInitialized = false;
        private static ContentManager content;
        private static GraphicsDevice graphics;
        private static Engine engine;

        public static HashList<Shot> PlayerShots = new HashList<Shot>("playershots");
        public static HashList<Shot>  EnemyShots = new HashList<Shot>("enemyshots");
        public static HashList<Gib>      GibList = new HashList<Gib>("giblist");
        public static HashList<Enemy>  EnemyList = new HashList<Enemy>("enemylist");
        public static HashList<Enemy>  DyingList = new HashList<Enemy>("dyinglist");
        public static HashList<Item>    ItemList = new HashList<Item>("itemlist");

        public static ScoreMenu scoreMenu;

        public static IContext Top { get { return stack.Peek(); } }

        public static void Initialize(Engine engine, ContentManager content, GraphicsDevice graphics)
        {
            World.content = content;
            World.graphics = graphics;
            World.engine = engine;
            isInitialized = true;
        }

        public static void Update(ControlManager controls)
        {
            stack.Peek().Update(controls);
        }
        
        public static void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            stack.Peek().Draw(graphics, batch);
        }

        public static void UpdateParent(ControlManager controls)
        {
            IContext saved = stack.Pop();
            Level level = (Level)stack.Peek();
            level.Update(controls, false);
            stack.Push(saved);
        }

        public static void DrawParent(GraphicsDevice graphics, SpriteBatch batch)
        {
            IContext saved = stack.Pop();
            stack.Peek().Draw(graphics, batch);
            stack.Push(saved);
        }

        public static void Process(ContextEvent ent)
        {

        }

        public static void Process(BackEvent ent)
        {
            Top.Unload();
            stack.Pop();
        }

        public static void Process(ScoreEvent ent)
        {
            if (!(Top is ScoreMenu)) throw new ExeggcuteError("can only be called form the score menu");
            ScoreMenu scoreMenu = (ScoreMenu)Top;
            ScoreEventType type = ent.Type;
            if (type == ScoreEventType.SeeLocal)
            {
                scoreMenu.ShowLocal();
            }
            else if (type == ScoreEventType.SeeNetwork)
            {
                scoreMenu.ShowNetwork();
            }
            else if (type == ScoreEventType.Submit)
            {
                scoreMenu.SyncNetwork();
            }
        }

        public static void Process(ToScoresEvent ent)
        {
            if (scoreMenu == null)
            {
                SpriteFont font = FontBank.Get("consolas");
                scoreMenu = new ScoreMenu(font);
            }
            stack.Push(scoreMenu);
        }

        public static void Process(LoadLevelEvent ent)
        {
            LevelLoader loader = new LevelLoader();
            PlayerLoader playerLoader = new PlayerLoader();
            Player player = playerLoader.Load("0");
            Level newLevel = loader.Load(content, graphics, player, "0");
            stack.Push(newLevel);
        }

        public static void Process(QuitEvent ent)
        {
            engine.Exit();
        }
        
        // Do I need the events at all if im doing it this way?
        public static void SendMove(Direction dir)
        {
            ((Menu)(stack.Peek())).Move(dir);
        }

        public static void Pop(/*IContext self*/)
        {
            if (true /*|| self == stack.Peek()*/)
            {
                stack.Pop();
            }
            else
            {
                throw new InvalidOperationException("Can only pop yourself");
            }
        }

        public static void SendAccept()
        {
            
        }
        
        public static void SendBack()
        {
          
        }

        public static void Pause()
        {
            stack.Push(new PauseMenu());
        }

        public static void Unpause()
        {
            if (!(stack.Peek() is PauseMenu))
            {
                throw new InvalidOperationException("Only can unpause from the pause menu");
            }
            stack.Pop();
        }

        public static void PushContext(IContext context)
        {
            stack.Push(context);
        }
        
        /// <summary>
        /// Removes all elements from the stack, calling their Cleanup method
        /// as they are removed.
        /// </summary>
        public static void Cleanup(ContentManager content)
        {
            for (int i = 0; i < stack.Count; i += 1)
            {
                stack.Pop().Unload();
            }
        }



    }
}
