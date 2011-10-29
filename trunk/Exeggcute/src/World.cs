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
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src
{
    /// <summary>
    /// The world is modeled as a stack of "Contexts" which can be pushed on
    /// top of one another. For instance, the "Level" context may be pushed 
    /// onto the "Main menu" context when the player begins the game, or the
    /// "Pause menu" may be pushed onto the "Level" context when the player
    /// pauses.
    /// 
    /// Generally only the topmost context is processed, but a context may
    /// choose to allow its parent to be processed as well.
    /// 
    /// Currently menus are hardcoded because I can't be bothered to make
    /// a specification language for them (and its beyond the scope of 
    /// the project anyhow)
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

        public static IContext Top { get { return stack.Peek(); } }

        public static WangMesh Terrain;
        private static WangMesh menuTerrain;
        // We cache menus so they are loaded once when they are first seen, then
        // re-used later when they are called upon.
        private static MainMenu mainMenu;
        private static ScoreMenu scoreMenu;
        private static PauseMenu pauseMenu;
        private static ReallyQuitMenu reallyQuitMenu;
        private static DifficultyMenu difficultyMenu;
        private static PlayerMenu standardPlayerMenu;
        private static PlayerMenu customPlayerMenu;

        private static LevelLoader levelLoader = new LevelLoader();

        // As we traverse through the menus, we build settings which are 
        // sufficient for building a Level instance.
        private static Difficulty difficulty;
        private static GameType gameType;

        public static Camera camera = new Camera(100, MathHelper.PiOver2, 0.1f);

        public static void Initialize(Engine engine, ContentManager content, GraphicsDevice graphics)
        {
            World.content = content;
            World.graphics = graphics;
            World.engine = engine;
            isInitialized = true;
        }


        private static VisualizationData soundData = new VisualizationData();
        public static void Update(ControlManager controls)
        {
            MediaPlayer.GetVisualizationData(soundData);
            Terrain.Update(soundData.Frequencies);
            stack.Peek().Update(controls);
        }
        
        public static void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            if (Terrain == menuTerrain)
            {

                Terrain.DrawRot(graphics, camera.GetView(), camera.GetProjection(), 0.0001f);
            }
            else
            {
                Terrain.Draw(graphics, camera.GetView(), camera.GetProjection());
            
            }
            stack.Peek().Draw(graphics, batch);
        }

        /// <summary>
        /// Called by a context when it wants to allow its parent to be
        /// updated.
        /// </summary>
        public static void UpdateParent(ControlManager controls)
        {
            IContext saved = stack.Pop();
            Level level = (Level)stack.Peek();
            level.Update(controls, false);
            stack.Push(saved);
        }

        /// <summary>
        /// Called by a context when it wants to allow its parent to be
        /// drawn.
        /// </summary>
        public static void DrawParent(GraphicsDevice graphics, SpriteBatch batch)
        {
            IContext saved = stack.Pop();
            stack.Peek().Draw(graphics, batch);
            stack.Push(saved);
        }

        public static void Process(ContextEvent ent)
        {
            Util.Warn("{0} event not implemented", ent.GetType());
        }

        public static void Process(ToPlayerMenuEvent ent)
        {
            difficulty = ent.Setting;
            
            Rectangle bounds = new Rectangle(50, 500, 100, 100);
            SpriteFont font = FontBank.Get("consolas");
            Color fontColor = Color.Black;
            //FIXME: make player select menu
            bool isCustom = (gameType == GameType.Custom);
            List<Player> players = PlayerBank.GetAll(isCustom).ToList();
            if (isCustom)
            {
                if (customPlayerMenu == null)
                {
                    List<Button> buttons = PlayerMenu.MakeButtons(font, fontColor, true);
                    
                    customPlayerMenu = new PlayerMenu(players, buttons, bounds, isCustom);
                }
                stack.Push(customPlayerMenu);
                return;
            }
            else
            {
                if (standardPlayerMenu == null)
                {
                    List<Button> buttons = PlayerMenu.MakeButtons(font, fontColor, isCustom);
                    standardPlayerMenu = new PlayerMenu(players, buttons, bounds, isCustom);
                }
                stack.Push(standardPlayerMenu);
                return;
            }
        }

        public static void Process(BackEvent ent)
        {
            World.Back();
        }

        /// <summary>
        /// Called in the pause menu to *un*pause back to the game.
        /// </summary>
        public static void Process(PauseEvent ent)
        {
            stack.Pop();
            if (!(Top is Level))
            {
                Util.Warn("you can only call unpause from a level, because you can only pause in a level!");
            }
        }

        /// <summary>
        /// Called when going from main to difficulty select menu.
        /// </summary>
        public static void Process(ToDifficultyMenuEvent ent)
        {
            gameType = ent.GameType;
            if (difficultyMenu == null)
            {
                SpriteFont font = FontBank.Get("consolas");
                Color fontColor = Color.Black;
                Rectangle bounds = new Rectangle(500, 500, 100, 116);
                List<Button> buttons = new List<Button> {
                    new ListButton(new ToPlayerMenuEvent(Difficulty.Easy), new SpriteText(font, "Easy", fontColor)),
                    new ListButton(new ToPlayerMenuEvent(Difficulty.Normal), new SpriteText(font, "Normal", fontColor)),
                    new ListButton(new ToPlayerMenuEvent(Difficulty.Hard), new SpriteText(font, "Hard", fontColor)),
                    new ListButton(new ToPlayerMenuEvent(Difficulty.VHard), new SpriteText(font, "V-Hard", fontColor)),
                };
                difficultyMenu = new DifficultyMenu(buttons, bounds);
            }
            stack.Push(difficultyMenu);
        }

        /// <summary>
        /// Called when returning to the main menu from anywhere at all.
        /// Pops things off the stack until we find the main menu, cleaning
        /// them up as we go.
        /// </summary>
        /// <param name="ent"></param>
        public static void Process(ToMainMenuEvent ent)
        {
            int count = stack.Count;//stack.Count is a property, not a constant
            for (int i = 0; i < count; i += 1)
            {
                if (Top is MainMenu)
                {
                    break;
                }
                Top.Unload();
                stack.Pop();
            }
        }

        public static void Process(ReallyQuitEvent ent)
        {
            if (reallyQuitMenu == null)
            {
                Color fontColor = Color.Black;
                SpriteFont font = FontBank.Get("consolas");
                List<Button> buttons = new List<Button> {
                    new ListButton(null, new SpriteText(font, "Yes", fontColor)),
                    new ListButton(new BackEvent(), new SpriteText(font, "No", fontColor)),
                };
                Rectangle bounds = new Rectangle(500, 500, 100, 100);
                reallyQuitMenu = new ReallyQuitMenu(buttons, bounds);
            }
            reallyQuitMenu.Initialize(ent.Type);
            stack.Push(reallyQuitMenu);
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
                Color fontColor = Color.Black;
                SpriteFont font = FontBank.Get("consolas");
                List<Button> buttons = new List<Button> {
                    new ListButton(new ScoreEvent(ScoreEventType.SeeLocal), new SpriteText(font, "View Local", fontColor)),
                    new ListButton(new ScoreEvent(ScoreEventType.SeeNetwork), new SpriteText(font, "View Network", fontColor)),
                    new ListButton(new ScoreEvent(ScoreEventType.Submit), new SpriteText(font, "Submit", fontColor)),
                    new ListButton(new BackEvent(), new SpriteText(font, "Back", fontColor)),
                };
                Rectangle bounds = new Rectangle(500, 500, 190, 190);
                scoreMenu = new ScoreMenu(buttons, bounds);
            }
            stack.Push(scoreMenu);
        }

        public static void Process(LoadLevelEvent ent)
        {
            string levelName = ent.LevelName;
            string playerName = ent.PlayerName;
            bool isCustom = ent.IsCustom;
            Player player = PlayerBank.Get(playerName, isCustom);
            HUD hud = new HUD();
            LoadNextLevel(hud, player, levelName, false);
        }

        public static void CleanupLevel()
        {
            Level level = (Level)stack.Peek();

            if (level.DoneCleanup())
            {
                level.Unload();
                stack.Pop();
                stack.Push(new LevelSummaryMenu(level));
            }
        }


        /// <summary>
        /// doPop should be true iff we are loading a level directly from a LevelSummaryMenu
        /// </summary>
        public static void LoadNextLevel(HUD hud, Player player, string name, bool doPop)
        {
            ClearLists();
            if (name.Equals("1"))//FIXME hard coded
            {

            }
            Level next = levelLoader.Load(content, graphics, player, hud, difficulty, name);


            if (doPop)
            {
                stack.Pop();
            }
            stack.Push(next);
        }

        public static void ClearLists()
        {
            PlayerShots.Clear();
            EnemyShots.Clear();
            EnemyList.Clear();
            GibList.Clear();
            DyingList.Clear();
            ItemList.Clear();
        }

        public static void Process(ExitGameEvent ent)
        {
            engine.Exit();
        }
        
        // Do I need the events at all if im doing it this way?
        public static void SendMove(Direction dir)
        {
            //fixme this is ugly
            Menu current = (Menu)stack.Peek();
            current.Move(dir);
            if (current.ResolveCursor())
            {
                SfxBank.Get("menumove").Play();
            }
            

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
        private static WangMesh savedTerrain;
        public static void Pause()
        {
            savedTerrain = Terrain;
            Terrain = menuTerrain;
            if (pauseMenu == null)
            {
                SpriteFont font = FontBank.Get("consolas");
                Color fontColor = Color.Black;
                Rectangle bounds = new Rectangle(500, 500, 100, 100);
                List<Button> buttons = new List<Button> {
                    new ListButton(new PauseEvent(), new SpriteText(font, "Continue", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.MainMenu), new SpriteText(font, "Quit to Main Menu", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.ExitGame), new SpriteText(font, "Exit game", fontColor)),
                };
                pauseMenu = new PauseMenu(buttons, bounds);
            }
            if (Top is Level)
            {
                
            }
            else
            {
                throw new ExeggcuteError("can only pause from a level!");
            }

            stack.Push(pauseMenu);
        }

        public static void Unpause()
        {
            if (!(stack.Peek() is PauseMenu))
            {
                throw new InvalidOperationException("Only can unpause from the pause menu");
            }
            Terrain = savedTerrain;
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

        public static void Back()
        {
            Top.Unload();
            stack.Pop();
        }


        public static void Begin()
        {
            TerrainInfo info = new TerrainInfo("data/world/bg.terrain");
            menuTerrain = info.MakeMesh(graphics);
            Terrain = menuTerrain;
            if (mainMenu == null)
            {
                Color fontColor = Color.Black;
                SpriteFont font = FontBank.Get("consolas");
                Rectangle bounds = new Rectangle(500, 500, 166, 150);
                List<Button> buttons = new List<Button> {
                    new ListButton(new ToDifficultyMenuEvent(GameType.Campaign),
                                   new SpriteText(font, "Campaign", fontColor)),
                    new ListButton(new ToDifficultyMenuEvent(GameType.Arcade),
                                   new SpriteText(font, "Arcade", fontColor)),

                    new ListButton(new ToDifficultyMenuEvent(GameType.Custom),
                                   new SpriteText(font, "Custom", fontColor)),

                    new ListButton(new ToScoresEvent(),
                                   new SpriteText(font, "High Scores", fontColor)),
                    new ListButton(new ExitGameEvent(),
                                   new SpriteText(font, "Quit", fontColor))
            };
                mainMenu = new MainMenu(buttons, bounds);
            }
            World.PushContext(mainMenu);
        }
    }
}
