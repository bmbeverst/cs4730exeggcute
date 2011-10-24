using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Exeggcute.src.particles;
using Exeggcute.src.entities;
using Exeggcute.src.assets;
using Exeggcute.src.text;
using Exeggcute.src.scripting.task;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.physics;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;
using Exeggcute.src.loading.specs;

namespace Exeggcute.src
{

    /// <summary>
    /// Base class for game "levels". If there is only one persistent
    /// level in our game, then that counts too.
    /// </summary>
    class Level : IContext
    {
        public HUD Hud { get; protected set; }
        public string Name { get; protected set; }
        public Difficulty Difficulty { get; protected set; }

        /// <summary>
        /// If the user has made no customizations, we will save the
        /// high scores.
        /// </summary>
        public bool ValidScore { get; protected set; }
        private ParticleSystem particles;
        private Camera camera;
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

        private TaskListLoader loader = new TaskListLoader();

        private VisualizationData soundData = new VisualizationData();

        private GrowBox shotEater = null;

        private Boss mainBoss = null;
        private Boss miniBoss = null;

        private Boss boss = null;

        private Song levelTheme = null;
        private Song bossTheme = null;

        //FIXME put a lot of this stuff in Load!
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
                     WangMesh terrain)
        {
            MediaPlayer.IsVisualizationEnabled = true;

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
            this.levelTheme  = levelTheme;
            this.bossTheme   = bossTheme;

            this.collider = new EntityManager();
            this.physics  = new PhysicsManager();
            this.camera   = new Camera(100, MathHelper.PiOver2, 1);
            this.Hud      = hud;

            Hud.DoFade(FadeType.In);

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            //TODO parse the player file here
            Level.player = player;

            MediaPlayer.Play(levelTheme);
            MediaPlayer.Pause();
        }

        private void updateShots(params HashList<Shot>[] lists)
        {
            foreach (HashList<Shot> shots in lists)
            {
                List<Shot> toRemove = new List<Shot>();
                foreach (Shot shot in shots.GetKeys())
                {
                    shot.Update();
                    if (!shot.ContainedIn(LiveArea))
                    {
                        toRemove.Add(shot);
                    }
                }
                toRemove.ForEach(shot => shots.Remove(shot));
            }
        }

        public void Process(Task task)
        {
            throw new InvalidOperationException("Must call a subclass overload");
        }

        public void Process(MessageTask task)
        {
            World.PushContext(new Conversation(boxes[task.ID]));
            taskPtr += 1;
        }

        public void Process(SpawnTask task)
        {
            Enemy toSpawn = roster.Clone(task.ID, task.Args);
            enemyList.Add(toSpawn);
            taskPtr += 1;
        }

        protected int counter = 0;
        public void Process(WaitTask task)
        {
            if (counter >= task.Duration)
            {
                counter = 0;
                taskPtr += 1;
            }
            counter += 1;
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
                boss.SetPosition(bossTask.Position.Vector);
            }
            taskPtr += 1;
        }

        public void ProcessTasks()
        {
            if (taskPtr >= taskList.Count) return;
            Task current = taskList[taskPtr];
            current.Process(this);
        }

        public void Update(ControlManager controls)
        {
            Update(controls, true);
        }

        public void Update(ControlManager controls, bool playerCanShoot)
        {
            MediaPlayer.GetVisualizationData(soundData);
            Hud.Update();
            //camera.Update(controls);
            ProcessTasks();
            particles.Update();
            terrain.Update(soundData.Frequencies);
            terrain.Impact(player.X, player.Y, 0, 0);
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
            collider.Collide(itemList, player);

            collider.FilterOffscreen(playerShots, LiveArea);
            collider.FilterOffscreen(gibList, LiveArea);
            collider.FilterOffscreen(enemyShots, LiveArea);
            collider.FilterOffscreen(itemList, LiveArea);

            // =[
            collider.FilterDead(playerShots);
            collider.FilterDead(enemyShots);
            collider.FilterDead(gibList);
            collider.FilterDead(enemyList);
            collider.FilterDead(World.DyingList);
            collider.FilterDead(World.ItemList);


            player.Update(controls, playerCanShoot);
            player.LockPosition(camera, GameArea);


            updateEntityList(enemyList);
            updateEntityList(gibList);
            updateEntityList(itemList);
            updateEntityList(playerShots);
            updateEntityList(enemyShots);

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

        private void drawEntityList<TEntity>(GraphicsDevice graphics, Matrix view, Matrix projection, HashList<TEntity> entities) 
            where TEntity : Entity3D
        {
            foreach (TEntity entity in entities.GetKeys())
            {
                entity.Draw(graphics, view, projection);
            }
        }

        private void updateEntityList<TEntity>(HashList<TEntity> entities)
            where TEntity : Entity3D
        {
            foreach (TEntity entity in entities.GetKeys())
            {
                entity.Update();
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            terrain.Draw(graphics, view, projection);

            player.Draw(graphics, view, projection);

            drawEntityList(graphics, view, projection, enemyList);
            drawEntityList(graphics, view, projection, gibList);
            drawEntityList(graphics, view, projection, itemList);
            drawEntityList(graphics, view, projection, enemyShots);
            drawEntityList(graphics, view, projection, playerShots);

            if (boss != null)
            {
                boss.Draw(graphics, view, projection);
            }

            particles.SetCamera(view, projection);
            particles.Draw(graphics);

            batch.Begin();
            Hud.Draw(batch, player);
            batch.End();
            
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

        public void Load(ContentManager content)
        {

        }

        public void Unload()
        {
            miniBoss.Reset();
            mainBoss.Reset();
        }

        public void Dispose()
        {

        }
    }
}
