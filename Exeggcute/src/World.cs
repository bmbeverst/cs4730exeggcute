using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Exeggcute.src.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.console.trackers;
using Exeggcute.src.config;

namespace Exeggcute.src
{
    static class Worlds
    {
        public static World World;
        public static void Reset(Engine engine, ContentManager content, GraphicsDevice graphics)
        {
            World = new World(engine, content, graphics);
        }

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
        private DevConsole console;

        private ContextStack stack = new ContextStack();
        private bool isInitialized;
        public ContentManager Content;
        public GraphicsDevice Graphics;
        private Engine engine;

        private HashList<Shot> playerShots = new HashList<Shot>("playershots");
        private HashList<Shot> enemyShots = new HashList<Shot>("enemyshots");
        private HashList<Gib> gibList = new HashList<Gib>("giblist");
        private HashList<Enemy> enemyList = new HashList<Enemy>("enemylist");
        private HashList<Enemy> dyingList = new HashList<Enemy>("dyinglist");
        private HashList<Item> itemList = new HashList<Item>("itemlist");
        private Dictionary<int, Entity3D> trackedList = new Dictionary<int, Entity3D> ();



        private Dictionary<Alignment, HashList<Shot>> shotDict;

        public Dictionary<int, Entity3D> Entities = new Dictionary<int, Entity3D>();

        public IContext Top { get { return stack.Top; } }
        public IContext Front { get { return stack.Front; } }

        //public WangMesh Terrain;
        //private WangMesh menuTerrain;
        // We cache menus so they are loaded once when they are first seen, then
        // re-used later when they are called upon.
        private MainMenu mainMenu;
        private ScoreMenu scoreMenu;
        private PauseMenu pauseMenu;
        private ReallyQuitMenu reallyQuitMenu;
        private DifficultyMenu difficultyMenu;
        private PlayerMenu playerMenu;

        private SongManager songManager = new SongManager();

        // As we traverse through the menus, we build settings which are 
        // sufficient for building a Level instance.
        private Difficulty difficulty;
        private GameType gameType;

        public Camera camera = new Camera(100, MathHelper.PiOver2, 0.1f);

        private Campaign campaign = new Campaign("default");
        private int levelPtr = 0;

        

        private Player player;
        private HUD hud;

        private WangMesh menuTerrain;

        //protected ResourceBatcher batcher = new ResourceBatcher();

        private List<Tracker> trackers = new List<Tracker>();

        public World(Engine engine, ContentManager content, GraphicsDevice graphics)
        {
            this.Content = content;
            this.Graphics = graphics;
            this.engine = engine;
            this.isInitialized = true;

            shotDict =
                new Dictionary<Alignment, HashList<Shot>>
                { 
                    { Alignment.Player, playerShots },
                    { Alignment.Enemy, enemyShots }
                };
        }


        public void AssertInitialized()
        {
            if (!this.isInitialized) throw new InvalidOperationException();
        }

        public void Register(Entity3D entity)
        {
            

        }

        public void ResetLights()
        {
            Effect effect = Assets.Effect["light0"];
            effect.CurrentTechnique = effect.Techniques["Textured"];

            effect.Parameters["xAmbient"].SetValue(1);
        }

        public void AddTracker(Tracker tracker)
        {
            trackers.Add(tracker);
        }

        public DevConsole MakeOverlay()
        {
            if (console == null)
            {
                console = new DevConsole();
                stack.AttachConsole(console);
            }
            hud = new HUD();
            TerrainInfo info = new TerrainInfo("data/world/bg.terrain");
            menuTerrain = info.MakeMesh(Graphics);
            return console;
        }

        public void RunInit()
        {
            console.RunInit();
            ResetLights();
        }

        public IEnumerable<Enemy> GetDying()
        {
            return dyingList;
        }

        public void AddDying(Enemy entity)
        {
            dyingList.Add(entity);
        }

        public IEnumerable<Enemy> GetEnemies()
        {
            return enemyList;
        }

        public void AddEnemy(Enemy enemy)
        {
            enemyList.Add(enemy);
            Entities[enemy.ID] = enemy;
        }

        public void AddShot(Shot shot, Alignment alignment)
        {
            shotDict[alignment].Add(shot);
            Entities[shot.ID] = shot;
        }

        public IEnumerable<Shot> GetPlayerShots()
        {
            return playerShots;
        }

        public IEnumerable<Shot> GetEnemyShots()
        {
            return enemyShots;
        }

        public IEnumerable<Gib> GetGibList()
        {
            return gibList;
        }

        public void AddGib(Gib gib)
        {
            gibList.Add(gib);
            Entities[gib.ID] = gib;
        }

        public void ReleaseItems(ItemBatch items, Vector3 deathPos)
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

        public IEnumerable<Item> GetItemList()
        {
            return itemList;
        }


        
        public void Update(ControlManager controls)
        {
            bool toggled = false;
            if (controls[Ctrl.Console].DoEatPress())
            {
                toggled = true;
                stack.ToggleConsole();
            }

            trackers.ForEach(track => track.Update());
            songManager.Update(toggled);


            Front.Update(controls);
        }
        
        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Front.Draw3D(graphics, camera);

            batch.Begin();
            Front.Draw2D(batch);
            for (int i = 0; i < trackers.Count; i += 1)
            {
                trackers[i].Draw2D(batch, new Vector2(0, i * 12));
            }
            foreach (Entity3D hasTrackers in trackedList.Values)
            {
                hasTrackers.Draw2D(batch);
            }
            batch.End();
        }

        private void push(IContext context)
        {
            if (context is MainMenu)
            {
                ToMainMenu();
            }
            stack.Push(context);
        }

        private IContext pop()
        {
            return stack.Pop();
        }

        public void Process(ContextEvent ent)
        {
            Util.Warn("{0} event not implemented", ent.GetType());
        }

        public void Process(ToPlayerMenuEvent ent)
        {
            difficulty = ent.Setting;
            
            Rectangle bounds = new Rectangle(100, 500, 100, 100);
            SpriteFont font = Assets.Font["consolas"];
            Color fontColor = Color.Black;

            List<Player> players = Assets.Player.GetAssets();
            
            if (playerMenu == null)
            {
                List<Button> buttons = PlayerMenu.MakeButtons(font, fontColor);
                playerMenu = new PlayerMenu(players, buttons, menuTerrain, bounds);
            }
            push(playerMenu);
            return;
        }

        public void Process(BackEvent ent)
        {
            
            Back();
        }

        /// <summary>
        /// Called in the pause menu to *un*pause back to the game.
        /// </summary>
        public void Process(PauseEvent ent)
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
        public void Process(ToDifficultyMenuEvent ent)
        {
            Process(new ToPlayerMenuEvent(Difficulty.Normal));
            return;
            /*gameType = ent.GameType;
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
                difficultyMenu = new DifficultyMenu(buttons, menuTerrain, bounds);
            }
            push(difficultyMenu);*/
        }

        public void Unload()
        {
            console.Dispose();
        }

        /// <summary>
        /// Called when returning to the main menu from anywhere at all.
        /// Pops things off the stack until we find the main menu, cleaning
        /// them up as we go.
        /// </summary>
        /// <param name="ent"></param>
        public void Process(ToMainMenuEvent ent)
        {
            throw new ResetException(null);
        }

        public void Process(ReallyQuitEvent ent)
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
                reallyQuitMenu = new ReallyQuitMenu(buttons, menuTerrain, bounds);
            }
            reallyQuitMenu.Initialize(ent.Type);
            push(reallyQuitMenu);
        }

        public void Process(ScoreEvent ent)
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

        public void Process(ToScoresEvent ent)
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
                Rectangle bounds = new Rectangle(500, 600, 190, 120);
                scoreMenu = new ScoreMenu(buttons, menuTerrain, bounds);
            }
            push(scoreMenu);
        }

        
        public void Process(LoadLevelEvent ent)
        {
            string levelName = ent.LevelName;

            //fixme - why reload between levels? i know its cached but...
            string playerName = ent.PlayerName;

            setPlayer(playerName);
            if (levelName == "0")
            {
                foreach (Player player in Assets.Player.GetAssets())
                {
                    player.ResetFromDemo();
                }
            }
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

        public void DoFadeOut(int frames)
        {
            songManager.FadeOut(frames);
        }

        public bool CanPassBarrier(BarrierTask barrier)
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

        private bool prebossProcessing()
        {
            bool finished = songManager.State == SongManager.SongState.Off;
            if (finished)
            {
                Level level = (Level)Top;
                level.StartBoss();
            }
            return finished;
        }

        public void RequestPlay(Song song)
        {
            songManager.Play(song);
        }

        public void ResetMusic()
        {
            songManager.ResetState();
        }

        //fixme oh so gross
        public void CleanupLevel()
        {
            Level level = (Level)Top;

            if (level.DoneCleanup())
            {
                level.Unload();
                ClearLists();
                ResetMusic();
                ResetLights();
                pop();
                push(new LevelSummaryMenu(level, player, hud));
            }
        }

        //requires that name be a valid context to switch to
        //or sandbox is true
        public void ContextSwitch(string name, bool isSandbox)
        {
            ClearLists();
            while (!(Top is MainMenu))
            {
                
                Pop().Unload();
            }

            if (player == null || hud == null)
            {
                setPlayer(Player.DebugName);
                hud = new HUD();
            }

            IContext newContext;
            
            if (isSandbox)
            {
                Sandbox box = new Sandbox(menuTerrain);
                box.Attach(player, hud);
                newContext = box;
            }
            else
            {
                Level next = Assets.Level[name];
                next.Attach(player, hud);
                newContext = next;
            }


            push(newContext);

        }
        bool isGameOver = false;
        public void GameOver()
        {
            if (!isGameOver)
            {
                GameOverMenu menu = new GameOverMenu();
                Sandbox level = FindSandbox();
                menu.Attach(level.hud, level.player, level);
                push(menu);
                isGameOver = true;
            }
        }

        /// <summary>
        /// doPop should be true iff we are loading a level directly from a LevelSummaryMenu
        /// </summary>
        public void LoadNextLevel(HUD hud, Player player, bool doPop)
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

            push(next);
        }

        public void ClearLists()
        {
            playerShots.Clear();
            enemyShots.Clear();
            enemyList.Clear();
            gibList.Clear();
            dyingList.Clear();
            itemList.Clear();
        }

        private Player setPlayer(string name)
        {
            if (player != null)
            {
                Entities.Remove(player.ID);
            }
            player = Assets.Player[name];
            Entities[player.ID] = player;

            return player;
        }

        public void Process(ExitGameEvent ent)
        {
            engine.Exit();
        }
        
        // Do I need the events at all if im doing it this way?
        public void SendMove(Direction dir)
        {
            //fixme this is ugly
            Menu current = (Menu)Top;
            current.Move(dir);
            if (current.ResolveCursor())
            {
                Assets.Sfx["menumove"].Play(Settings.Global.Audio.SfxVolume, 0, 0);
            }
        }

        public IContext Pop(/*IContext self*/)
        {
            if (Top is MainMenu)
            {
                ToMainMenu();
            }
            return stack.Pop();
        }

        public void ToMainMenu()
        {
            songManager.Stop();
            songManager.Play(Assets.Song["Birth"]);
            ResetLights();
        }

        public void Pause()
        {
            if (!(Top is Level))
            {
                return;
            }

            songManager.Pause();
            if (pauseMenu == null)
            {
                SpriteFont font = Assets.Font["consolas"];
                Color fontColor = Color.Black;
                Rectangle bounds = new Rectangle(500, 500, 250, 100);
                List<Button> buttons = new List<Button> {
                    new ListButton(new PauseEvent(), new SpriteText(font, "Continue", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.MainMenu), new SpriteText(font, "Quit to Main Menu", fontColor)),
                    new ListButton(new ReallyQuitEvent(QuitType.ExitGame), new SpriteText(font, "Exit game", fontColor)),
                };
                pauseMenu = new PauseMenu(buttons, menuTerrain, bounds);
            }
            

            push(pauseMenu);
        }

        public void AttachTracker(Entity3D entity, Tracker tracker)
        {
            trackedList[entity.ID] = entity;
            entity.AddTracker(tracker);
        }

        protected void removeTracker(Entity3D entity)
        {
            entity.SurrenderTrackers();
            trackedList.Remove(entity.ID);
        }

        public void ClearTrackers()
        {
            foreach (Entity3D entity in trackedList.Values)
            {
                removeTracker(entity);
            }
            trackers = new List<Tracker>();
        }

        public void Unpause()
        {
            if (!(Top is PauseMenu))
            {
                throw new InvalidOperationException("Only can unpause from the pause menu");
            }
            songManager.Unpause();
            stack.Pop();
        }

        public void PushContext(IContext context)
        {
            if (context is MainMenu)
            {
                ToMainMenu();
            }
            push(context);
        }
        
        /// <summary>
        /// Removes all elements from the stack, calling their Cleanup method
        /// as they are removed.
        /// </summary>
        public void Cleanup(ContentManager content)
        {
            stack.Empty();
        }

        public void Back()
        {
            Top.Unload();
            stack.Pop();
        }

        public IContext getSecond()
        {
            IContext temp = stack.Pop();
            IContext result = Top;
            push(temp);
            return result;
        }

        public void InsertPlayer(string name)
        {
            if (!Assets.Player.ContainsKey(name))
            {
                console.WriteLine("No player named {0} found. Valid choices are:", name);
                console.AcceptCommand(new ListCommand(console, FileType.Player));
                return;
            }
            if (Top is Sandbox)
            {
                Sandbox sandbox = (Sandbox)Top;
                sandbox.AttachPlayer(setPlayer(name));
            }
            else
            {
                console.WriteLine("May only insert a player into a Level or Sandbox context. See 'help context'");
            }
        }

        public void InsertEnemy(string name, Float3 pos, FloatValue angle)
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
            AddEnemy(Assets.Enemy[name].Clone(pos, angle));
        }

        public void InsertBoss(string name)
        {
            console.WriteLine("TODO: not implemented");
        }

        public void Begin()
        {
            if (mainMenu == null)
            {
                Color fontColor = Color.Black;
                SpriteFont font = Assets.Font["consolas"];
                Rectangle bounds = new Rectangle(500, 600, 166, 100);
                List<Button> buttons = new List<Button> {
                    new ListButton(new ToDifficultyMenuEvent(GameType.Campaign),
                                   new SpriteText(font, "Campaign", fontColor)),
                    new ListButton(new ToScoresEvent(),
                                   new SpriteText(font, "High Scores", fontColor)),
                    new ListButton(new ExitGameEvent(),
                                   new SpriteText(font, "Quit", fontColor))
                };
                mainMenu = new MainMenu(buttons, menuTerrain, bounds);
            }
            push(mainMenu);
        }

        public void ConsoleWrite<T>(IEnumerable<T> lines)
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

        public void ConsoleWrite(string message, params object[] args)
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

        public void SendTask(List<Task> tasks)
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


        public Sandbox FindSandbox()
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
        public Level LoadLevelFromFile(string filename)
        {

            if (!isInitialized) throw new ExeggcuteError("World not initialized yet!");

            if (player == null)
            {
                setPlayer(Player.DebugName);
            }

            return Loaders.Level.LoadByFile(Content, Graphics, new HUD(), Difficulty.Normal, filename);
        }

        public List<Entity3D> FindPointedTo<T>(Vector2 mouseConverted, params IEnumerable<T>[] lists)
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

        public bool IsPointedTo(Entity3D entity, Vector2 mouseConverted)
        {
            return Util.SphereContains(entity.Position, entity.BaseModelRadius, mouseConverted);
        }

        public Entity3D GetUnderMouse(Vector2 mousePos)
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

        public void SetParameter(SetParamCommand set)
        {
            Entity3D toSet = Entities[set.ID];
            toSet.RawSetParam(set.ParamIndex, set.Value.Value);

        }

        public void FilterDead(EntityManager manager)
        {
            manager.FilterDead(playerShots, Entities);
            manager.FilterDead(enemyShots, Entities);
            manager.FilterDead(enemyList, Entities);
            manager.FilterDead(itemList, Entities);
            manager.FilterDead(gibList, Entities);
            manager.FilterDead(dyingList, Entities);
        }

        public void FilterOffscreen(EntityManager manager, Rectangle liveArea)
        {
            manager.FilterOffscreen(playerShots, liveArea, Entities);
            manager.FilterOffscreen(enemyShots, liveArea, Entities);
            manager.FilterOffscreen(itemList, liveArea, Entities);
            manager.FilterOffscreen(gibList, liveArea, Entities);
        }

        public void KillEnemies()
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Kill();
            }
        }

        public Vector3? GetPlayerPosition()
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

        internal void GiveScore(int p)
        {
            Sandbox level = FindSandbox();
            level.player.GivePoints(p);
        }
    }
}
