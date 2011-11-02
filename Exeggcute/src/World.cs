using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.console;
using Exeggcute.src.console.commands;
using Exeggcute.src.contexts;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.input;
using Exeggcute.src.loading;
using Exeggcute.src.scripting.task;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.text;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;

namespace Exeggcute.src
{
    static class Universe
    {

    }
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
    class World 
    {
        private static DevConsole console;

        private static Stack<IContext> stack;
        private static bool isInitialized;
        public static ContentManager Content;
        public static GraphicsDevice Graphics;
        private static Engine engine;

        private static HashList<Shot> playerShots;
        private static HashList<Shot> enemyShots;
        private static HashList<Gib> gibList;
        private static HashList<Enemy> enemyList;
        private static HashList<Enemy> dyingList;
        private static HashList<Item> itemList;

        private static Dictionary<Alignment, HashList<Shot>> shotDict;

        public static Dictionary <int, Entity3D> Entities;
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
        private static PlayerMenu playerMenu;

        private static SongManager songManager;

        // As we traverse through the menus, we build settings which are 
        // sufficient for building a Level instance.
        private static Difficulty difficulty;
        private static GameType gameType;

        public static Camera camera;

        private static Campaign campaign;

        private static VisualizationData soundData;

        private static Player player;
        private static HUD hud;

        public static void Initialize(Engine engine, ContentManager content, GraphicsDevice graphics)
        {
            World.Content = content;
            World.Graphics = graphics;
            World.engine = engine;
            isInitialized = true;
            
        }

        public static void Reset()
        {
            playerShots = new HashList<Shot>("playershots");
            enemyShots = new HashList<Shot>("enemyshots");
            gibList = new HashList<Gib>("giblist");
            enemyList = new HashList<Enemy>("enemylist");
            dyingList = new HashList<Enemy>("dyinglist");
            itemList = new HashList<Item>("itemlist");

            Entities = new Dictionary<int, Entity3D>();

            shotDict = new Dictionary<Alignment, HashList<Shot>>
            { 
                { Alignment.Player, playerShots },
                { Alignment.Enemy, enemyShots }
            };

            stack = new Stack<IContext>();

            songManager = new SongManager(0.1f);

            camera = new Camera(100, MathHelper.PiOver2, 0.1f);

            campaign = new Campaign("default");
            

            soundData = new VisualizationData();

            isInitialized = false;
            Content = null;
            Graphics = null;
            engine = null; 
            Terrain = null;
            menuTerrain = null;
            mainMenu = null; 
            scoreMenu = null;
            pauseMenu = null;
            reallyQuitMenu = null;
            difficultyMenu = null;
            playerMenu = null;
            savedTerrain = null;

            levelPtr = 0;
        }

        public static void PostInit()
        {
            hud = new HUD();
        }

        public static void AssertInitialized()
        {
            if (!World.isInitialized) throw new InvalidOperationException();
        }

        public static DevConsole MakeConsole()
        {
            if (console == null) console = new DevConsole();
            consoleAttached = false;
            return console;
        }

        public static IEnumerable<Enemy> GetDying()
        {
            return dyingList;
        }

        public static void AddDying(Enemy entity)
        {
            dyingList.Add(entity);
        }

        public static IEnumerable<Enemy> GetEnemies()
        {
            return enemyList;
        }

        public static void AddEnemy(Enemy enemy)
        {
            enemyList.Add(enemy);
            Entities[enemy.ID] = enemy;
        }

        public static void AddShot(Shot shot, Alignment alignment)
        {
            shotDict[alignment].Add(shot);
            Entities[shot.ID] = shot;
        }

        public static IEnumerable<Shot> GetPlayerShots()
        {
            return playerShots;
        }

        public static IEnumerable<Shot> GetEnemyShots()
        {
            return enemyShots;
        }

        public static IEnumerable<Gib> GetGibList()
        {
            return gibList;
        }

        public static void AddGib(Gib gib)
        {
            gibList.Add(gib);
            Entities[gib.ID] = gib;
        }

        public static void ReleaseItems(ItemBatch items, Vector3 deathPos)
        {
            Float3 dispersion = new Float3(new FloatRange(0, 5), new FloatRange(0, 5), new FloatValue(0));
            foreach (Item item in items.myItems)
            {
                Vector3 pos = dispersion.Vector3;
                item.SetPosition(deathPos + pos);
                itemList.Add(item);
                Entities[item.ID] = item;
            }
        }

        public static IEnumerable<Item> GetItemList()
        {
            return itemList;
        }


        static bool consoleAttached = false;
        public static void Update(ControlManager controls)
        {
            if (controls[Ctrl.Console].DoEatPress())
            {
                if (consoleAttached)
                {
                    stack.Pop();
                    console.DetachParent();
                    consoleAttached = false;
                    MediaPlayer.Resume();
                    
                }
                else
                {
                    console.AttachParent(Top);
                    stack.Push(console);
                    consoleAttached = true;
                    MediaPlayer.Pause();
                }
            }
            songManager.Update();
            MediaPlayer.GetVisualizationData(soundData);
            Terrain.Update(soundData.Frequencies);
            
            Top.Update(controls);
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

            Top.Draw3D(graphics, camera);

            batch.Begin();
            Top.Draw2D(batch);
            batch.End();
        }

        public static void Process(ContextEvent ent)
        {
            Util.Warn("{0} event not implemented", ent.GetType());
        }

        public static void Process(ToPlayerMenuEvent ent)
        {
            difficulty = ent.Setting;
            
            Rectangle bounds = new Rectangle(100, 500, 100, 100);
            SpriteFont font = Assets.Font["consolas"];
            Color fontColor = Color.Black;

            List<Player> players = Assets.Player.GetAssets();
            
            if (playerMenu == null)
            {
                List<Button> buttons = PlayerMenu.MakeButtons(font, fontColor);
                playerMenu = new PlayerMenu(players, buttons, bounds);
            }
            stack.Push(playerMenu);
            return;
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
                SpriteFont font = Assets.Font["consolas"];
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
                SpriteFont font = Assets.Font["consolas"];
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
                SpriteFont font = Assets.Font["consolas"];
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

        static int levelPtr = 0;
        public static void Process(LoadLevelEvent ent)
        {
            string levelName = ent.LevelName;

            //fixme - why reload between levels? i know its cached but...
            string playerName = ent.PlayerName;

            setPlayer(playerName);
           
            HUD hud = new HUD();
            if (gameType == GameType.Campaign)
            {
                LoadNextLevel(hud, player, false);
            }
            else
            {
                //FIXME hard code
                LoadNextLevel(hud, player, false);
            }
        }

        public static void DoFadeOut(int frames)
        {
            songManager.FadeOut(frames);
        }

        public static bool CanPassBarrier(BarrierTask barrier)
        {
            BarrierType type = barrier.Type;
            if (type == BarrierType.FadeOut)
            {

                return prebossProcessing();
            }
            else
            {
                throw new ExeggcuteError("Dont know what to do with barrier of type {0}", type);
            }
        }

        private static bool prebossProcessing()
        {
            bool finished = songManager.State == SongManager.SongState.Off;
            if (finished)
            {
                Level level = (Level)Top;
                level.StartBoss();
            }
            return finished;
        }

        public static void RequestPlay(Song song)
        {
            songManager.Play(song);
        }

        public static void ResetMusic()
        {
            songManager.ResetState();
        }

        public static void CleanupLevel()
        {
            Level level = (Level)stack.Peek();

            if (level.DoneCleanup())
            {
                level.Unload();
                stack.Pop();
                stack.Push(new LevelSummaryMenu(level, player, hud));
            }
        }

        //requires that name be a valid context to switch to
        //or sandbox is true
        public static void ContextSwitch(string name, bool isSandbox)
        {
            if (!(Top is DevConsole))
            {
                throw new ExeggcuteError("impossible");
            }
            else
            {
                stack.Pop();
            }
            while (!(Top is MainMenu))
            {
                if (stack.Count == 0) throw new ExeggcuteError("main menu expected on top!");
                if (Top is Level)
                {
                    //unload level
                    Level level = (Level)Top;
                    ClearLists();
                    level.Unload();
                }
                stack.Pop();
            }

            if (player == null || hud == null)
            {
                setPlayer(Player.DebugName);
                hud = new HUD();
            }

            IContext newContext;
            
            if (isSandbox)
            {
                Terrain = menuTerrain;
                newContext = new Sandbox();
            }
            else
            {
                Level next = Assets.Level[name];
                newContext = next;
            }


            console.AttachParent(newContext);
            stack.Push(newContext);
            stack.Push(console);
        }

        /// <summary>
        /// doPop should be true iff we are loading a level directly from a LevelSummaryMenu
        /// </summary>
        public static void LoadNextLevel(HUD hud, Player player, bool doPop)
        {
            ClearLists();

            if (levelPtr == campaign.Count)
            {
                //FIXME go to main menu
                Engine.scoreSet.TryInsert(player.Score);
                Engine.scoreSet.WriteLocal();
                throw new ResetException(null);
                
            }

            Level next = Assets.Level[campaign[levelPtr]];

            next.Attach(player, hud);

            levelPtr += 1;

            if (doPop)
            {
                stack.Pop();
            }

            stack.Push(next);
        }

        public static void ClearLists()
        {
            playerShots.Clear();
            enemyShots.Clear();
            enemyList.Clear();
            gibList.Clear();
            dyingList.Clear();
            itemList.Clear();
        }

        private static Player setPlayer(string name)
        {
            if (player != null)
            {
                Entities.Remove(player.ID);
            }
            player = Assets.Player[name];
            Entities[player.ID] = player;

            return player;
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
                Assets.Sfx["menumove"].Play();
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
            if (!(Top is Level))
            {
                return;
            }

            savedTerrain = Terrain;
            Terrain = menuTerrain;
            songManager.Pause();
            if (pauseMenu == null)
            {
                SpriteFont font = Assets.Font["consolas"];
                Color fontColor = Color.Black;
                Rectangle bounds = new Rectangle(500, 500, 100, 100);
                List<Button> buttons = new List<Button> {
                    new ListButton(new PauseEvent(), new SpriteText(font, "Continue", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.MainMenu), new SpriteText(font, "Quit to Main Menu", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.ExitGame), new SpriteText(font, "Exit game", fontColor)),
                };
                pauseMenu = new PauseMenu(buttons, bounds);
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
            songManager.Unpause();
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

        public static IContext getSecond()
        {
            IContext temp = stack.Pop();
            IContext result = stack.Peek();
            stack.Push(temp);
            return result;
        }

        public static void InsertPlayer(string name)
        {
            if (!Assets.Player.ContainsKey(name))
            {
                console.WriteLine("No player named {0} found. Valid choices are:", name);
                console.AcceptCommand(new ListCommand(console, FileType.Player));
                return;
            }
            IContext second = getSecond();
            if (second is Sandbox)
            {
                Sandbox sandbox = (Sandbox)second;
                sandbox.Player = setPlayer(name);
            }
            else
            {
                console.WriteLine("May only insert a player into a Level or Sandbox context. See 'help context'");
            }
        }

        public static void InsertEnemy(string name, Float3 pos, FloatValue angle)
        {
            if (!Assets.Enemy.ContainsKey(name))
            {
                console.WriteLine("No enemy exists with that name. Valid enemies are:");
                console.WriteLines(Assets.Enemy.GetLoadedNames());
                return;
            }
            if (!(getSecond() is Sandbox) && false)
            {
                console.WriteLine("May only insert an enemy into a Level or Sandbox context. See 'help context'");
                return;
            }
            enemyList.Add(Assets.Enemy[name].Clone(pos, angle));
        }

        public static void InsertBoss(string name)
        {
            console.WriteLine("TODO: not implemented");
        }

        public static void Begin()
        {
            TerrainInfo info = new TerrainInfo("data/world/bg.terrain");
            menuTerrain = info.MakeMesh(Graphics);
            Terrain = menuTerrain;
            if (mainMenu == null)
            {
                Color fontColor = Color.Black;
                SpriteFont font = Assets.Font["consolas"];
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
            stack.Push(mainMenu);
        }

        public static void ConsoleWrite<T>(IEnumerable<T> lines)
        {
            if (console != null)
            {
                console.WriteLines(lines);
            }
            else
            {
                foreach (T obj in lines)
                {
                    Console.WriteLine(obj);
                }
            }
        }

        public static void ConsoleWrite(string message, params object[] args)
        {
            string formatted = string.Format(message, args);
            if (console == null)
            {
                Console.WriteLine(formatted);
            }
            else
            {
                console.WriteLine(formatted);
            }
        }

        public static void SendTask(List<Task> tasks)
        {
            IContext second = getSecond();
            Sandbox valid = FindSandbox();
            if (valid != null)
            {
                valid.AcceptTaskList(tasks);

            }
            else
            {
                throw new InvalidOperationException();
            }
        }


        public static Sandbox FindSandbox()
        {
            Dictionary<IContext, bool> seen = new Dictionary<IContext, bool>();
            IContext current = Top;
            while (current != null)
            {
                if (current is Sandbox)
                {
                    return current as Sandbox;
                }
                else
                {
                    current = current.Parent;
                }
            }
            return null;
        }
        public static Level LoadLevelFromFile(string filename)
        {

            if (!isInitialized) throw new ExeggcuteError("World not initialized yet!");

            if (player == null)
            {
                setPlayer(Player.DebugName);
            }

            return Loaders.Level.LoadByFile(Content, Graphics, new HUD(), Difficulty.Normal, filename);
        }

        public static List<Entity3D> FindPointedTo<T>(Vector2 mouseConverted, params IEnumerable<T>[] lists)
            where T : Entity3D
        {
            List<Entity3D> result = new List<Entity3D>();
            foreach (IEnumerable<T> list in lists)
            {
                foreach (T entity in list)
                {
                    if (entity == null) continue;
                    if (IsPointedTo(entity, mouseConverted))
                    {
                        result.Add(entity);
                    }
                }
            }
            return result;
        }

        /*public static Entity3D ExpensiveFindPointedTo<T>(Point mousePos, params IEnumerable<T>[] lists)
            where T : Entity3D
        {
            Entity3D min = null;
            float minDist = float.MaxValue;
            foreach (IEnumerable<T> list in lists)
            {
                foreach (T entity in list)
                {
                    if (min == null)
                    {
                        min = entity;
                        continue;
                    }
                    float curDist = Util.PointDistance(entity.Position, mousePos);
                    if (curDist < minDist)
                    {
                        curDist = minDist;
                        min = entity;
                    }
                }
            }
        }*/

        public static bool IsPointedTo(Entity3D entity, Vector2 mouseConverted)
        {
            return Util.SphereContains(entity.Position, entity.BaseModelRadius, mouseConverted);
        }

        public static Entity3D GetUnderMouse(Vector2 mousePos)
        {

            List<Entity3D> found =
                FindPointedTo<Entity3D>(mousePos, 
                    playerShots, 
                    enemyShots, 
                    gibList, 
                    enemyList, 
                    itemList,
                    new List<Entity3D> { player });

            Entity3D selected = found.OrderBy(entity => Util.PointDistance(entity.ModelHitbox.Center, mousePos)).FirstOrDefault();
            return selected;

        }

        public static void SetParameter(SetCommand set)
        {
            int index = Entity3D.NameToIndex(set.ParamName);
            if (index == -1)
            {
                Util.Die("This should be checked beforehand, right?");
            }
            else
            {
                Entity3D toSet = Entities[set.ID];
                toSet.RawSetParam(index, float.Parse(set.Value));
            }

        }

        public static void FilterDead(EntityManager manager)
        {
            manager.FilterDead(playerShots, Entities);
            manager.FilterDead(enemyShots, Entities);
            manager.FilterDead(enemyList, Entities);
            manager.FilterDead(itemList, Entities);
            manager.FilterDead(gibList, Entities);
            manager.FilterDead(dyingList, Entities);
        }

        public static void FilterOffscreen(EntityManager manager, Rectangle liveArea)
        {
            manager.FilterOffscreen(World.playerShots, liveArea, Entities);
            manager.FilterOffscreen(World.enemyShots, liveArea, Entities);
            manager.FilterOffscreen(World.itemList, liveArea, Entities);
            manager.FilterOffscreen(World.gibList, liveArea, Entities);
        }

        public static void KillEnemies()
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Kill();
            }
        }

        public static Vector3? GetPlayerPosition()
        {
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.Position;
            }
        }
    }
}
