using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.particles;
using Exeggcute.src.physics;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting.task;
using Exeggcute.src.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src
{

    /// <summary>
    /// Base class for game "levels". If there is only one persistent
    /// level in our game, then that counts too.
    /// </summary>
    class Level : ConsoleContext
    {
        public static HUD Hud;
        public string Name { get; protected set; }
        public Difficulty Difficulty { get; protected set; }

        /// <summary>
        /// If the user has made no customizations, we will save the
        /// high scores.
        /// </summary>
        public bool ValidScore { get; protected set; }
        private ParticleSystem particles;

        public static Player player;
        private EntityManager collider;
        private PhysicsManager physics;
        private Roster roster;
        private WangMesh terrain;

        private HashList<Enemy> enemyList;

        private HashList<Shot> playerShots;
        private HashList<Shot> enemyShots;
        private HashList<Gib> gibList;
        private HashList<Item> itemList;

        private List<Task> taskList;
        private int taskPtr;

        public static readonly int HalfWidth = 30;
        public static readonly int HalfHeight = 37;
        public Rectangle GameArea { get; protected set; }



        ///<summary>
        /// The percentage the LiveArea is increased to accomodate
        /// off-screen objects. 
        /// </summary>
        private float liveBuffer = 1f/4f;

        ///<summary> Outside of this area, enemies and shots are destroyed. </summary>
        public Rectangle LiveArea { get; protected set; }

        public List<TextBoxList> boxes = new List<TextBoxList>();

        private GrowBox shotEater;

        private Boss mainBoss;
        private Boss miniBoss;

        private Boss boss;

        private static Song levelTheme;
        private static Song bossTheme;

        public int initialScore;
        public bool IsStarted { get; protected set; }

        public Level(GraphicsDevice graphics, 
                     ContentManager content, 
                     Player player, 
                     HUD hud,
                     Difficulty difficulty,
                     bool validScore,
                     string name,
                     Roster roster, 
                     Song levelTheme, 
                     Song bossTheme,
                     Boss miniBoss, 
                     Boss mainBoss,
                     List<Task> tasks, 
                     WangMesh terrain,
                     LightSettings lightSettings)
        {
            MediaPlayer.IsVisualizationEnabled = true;
            //HACK HARDCODED
            Effect light = Assets.Effect["light0"];
            loadLights(lightSettings, light);

            this.terrain     = terrain;
            this.Name        = name;
            this.Difficulty  = difficulty;
            this.ValidScore  = validScore;
            this.playerShots = World.PlayerShots;
            this.enemyShots  = World.EnemyShots;
            this.enemyList   = World.EnemyList;
            this.gibList     = World.GibList;
            this.itemList    = World.ItemList;
            this.roster      = roster;
            this.taskList    = tasks;


            this.miniBoss    = miniBoss;
            this.mainBoss    = mainBoss;
            


            Level.levelTheme  = levelTheme;
            Level.bossTheme   = bossTheme;

            this.collider = new EntityManager();
            this.physics  = new PhysicsManager();
            Level.Hud      = hud;

            

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            //TODO parse the player file here
            Level.player = player;
            
            
        }

        public void Start()
        {
            Console.WriteLine("START {0}", Name);
            taskPtr = 0;
            initialScore = player.Score;
            Hud.DoFade(FadeType.In);
            player.SetPosition(Engine.Jail);
            player.ResetFromDemo();
            World.RequestPlay(levelTheme);
            World.Terrain = terrain;
            miniBoss.AttachConversations(this);
            mainBoss.AttachConversations(this);
            IsStarted = true;

        }


        public static Level LoadFromFile(string filename)
        {
            return World.LoadLevelFromFile(filename);
        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            throw new NotImplementedException();
        }

        public void Process(Task task)
        {
            throw new InvalidOperationException("Must call a subclass overload");
        }

        bool fadeStarted = false;
        public void Process(SongFadeTask task)
        {
            World.DoFadeOut(task.NumFrames);
            taskPtr += 1;
            
        }

        public void Process(BarrierTask barrier)
        {
            if (World.CanPassBarrier(barrier))
            {
                taskPtr += 1;
            }
        }

        public void Process(SpawnTask task)
        {
            Enemy toSpawn = roster.Clone(task.ID, task.Position, task.Angle);
            Console.WriteLine("{0} {1}", task.Position, task.Angle);
            enemyList.Add(toSpawn);
            taskPtr += 1;
            Console.WriteLine("    SPAWN! {0} {1}", taskPtr, Name);
        }

        protected int waitCounter = 0;
        public void Process(WaitTask task)
        {
            if (waitCounter >= task.Duration)
            {
                waitCounter = 0;
                taskPtr += 1;
            }
            waitCounter += 1;
        }

        public void Process(KillAllTask kill)
        {
            foreach (Enemy enemy in enemyList.GetKeys())
            {
                enemy.Kill();
            }
            taskPtr += 1;
        }

        public void Process(BossTask bossTask)
        {
            if (boss == null)
            {
                boss = miniBoss;
            }
            taskPtr += 1;
        }

        public void ProcessTasks()
        {
            if (taskPtr >= taskList.Count) return;
            Task current = taskList[taskPtr];
            current.Process(this);
        }

        public override void Update(ControlManager controls)
        {
            Update(controls, true);
        }

        public void Update(ControlManager controls, bool playerCanShoot)
        {
            //Console.WriteLine("{0} by {1}", Name, new StackFrame(3).GetMethod().DeclaringType.Name);
            if (!IsStarted) 
            { 
                Start(); 
                IsStarted = true; 
            }
            Hud.Update();
            //camera.Update(controls);
            ProcessTasks();
            particles.Update();

            for (int i = 0; i < 1; i += 1)
            {
                if (player.Velocity.Equals(Vector3.Zero)) break;
                particles.AddParticle(player.Position, -10*player.Velocity);
            }

            physics.Affect(gibList, true);
            //physics.Affect(playerShots, false);
            foreach (Gib gib in gibList.GetKeys())
            {
                collider.CollideTerrain(terrain, gib, GameArea);
            }

            physics.Affect(World.DyingList, true);
            collider.CollideDying(terrain);

            processHit();

            collider.Collide(playerShots, enemyList);
            collider.CollideItems(itemList, player);


            collider.UpdateAll(LiveArea);


            player.Update(controls, playerCanShoot);
            player.LockPosition(GameArea);

            if (boss != null)
            {
                collider.CollideBoss(playerShots, boss);
                boss.Update();
            }

        }

        /// <summary>
        /// What to do when the player gets hit
        /// </summary>
        private void processHit()
        {
            bool hit = collider.Collide(player, enemyList) ||
                       collider.HitPlayer(enemyShots, player);
            if (hit)
            {
                player.Kill();
                shotEater = new GrowBox(1);
            }
            if (shotEater != null)
            {
                shotEater.Update();
                collider.EatShots(enemyShots, shotEater.Rect);
                if (shotEater.Rect.Height > HalfHeight * 4)
                {
                    shotEater = null;
                }
            }
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();

            player.Draw(graphics, view, projection);

            collider.DrawAll(graphics, projection, view);

            if (boss != null)
            {
                boss.Draw(graphics, view, projection);
            }

            particles.SetCamera(view, projection);
            particles.Draw(graphics);
            
        }

        public override void Draw2D(SpriteBatch batch)
        {
            Hud.Draw(batch, player);
            if (boss != null) boss.Draw2D(batch);
        }

        bool cleanupStarted;
        public bool DoneCleanup()
        {
            if (!cleanupStarted)
            {
                Hud.DoFade(FadeType.Out);
                cleanupStarted = true;
            }
            if (!Hud.IsFading())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartBoss()
        {
            this.boss = mainBoss;

        }

        public void EndBossIntro(Boss boss)
        {
            World.RequestPlay(bossTheme);
        }

        public override void Unload()
        {
            boss = null; 
            shotEater = null;
            miniBoss.Reset();
            mainBoss.Reset();
            World.ClearLists();
            World.ResetMusic();
            taskPtr = 0;
            IsStarted = false;
        }

        public override void Dispose()
        {
            Unload();
        }


        private void loadLights(LightSettings settings, Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Textured"];


            if (settings.Point1On.Value)
            {
                effect.Parameters["xPointLight1"].SetValue(settings.Point1Pos.Value);
                effect.Parameters["xPointIntensity1"].SetValue(settings.Point1Level.Value);
                effect.Parameters["xPointIntensity1"].SetValue(settings.Point1Level.Value);

            }
            if (settings.DirOn.Value)
            {
                effect.Parameters["xLightDirection"].SetValue(settings.DirDirection.Value);
                effect.Parameters["xDirLightIntensity"].SetValue(settings.DirLevel.Value);
            }
            if (settings.SpotOn.Value)
            {
                effect.Parameters["xSpotPos"].SetValue(settings.SpotPos.Value);
                effect.Parameters["xSpotDir"].SetValue(settings.SpotDir.Value);
                effect.Parameters["xSpotInnerCone"].SetValue(settings.SpotInner.Value);
                effect.Parameters["xSpotOuterCone"].SetValue(settings.SpotOuter.Value);
                effect.Parameters["xSpotRange"].SetValue(settings.SpotRange.Value);
                effect.Parameters["xSpotIntensity"].SetValue(settings.SpotLevel.Value);
            }

            effect.Parameters["xAmbient"].SetValue(settings.AmbientLevel.Value);

        }
    }
}
