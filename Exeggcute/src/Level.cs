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

namespace Exeggcute.src
{

    /// <summary>
    /// Base class for game "levels". If there is only one persistent
    /// level in our game, then that counts too.
    /// </summary>
    class Level : IContext
    {
        private HUD hud;
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


        //FIXME put a lot of this stuff in Load!
        public Level(GraphicsDevice graphics, ContentManager content, Roster roster)
        {
            Song song = content.Load<Song>("songs/Mayhem_Some_Boss_Shit");
            
            MediaPlayer.Play(song);
            MediaPlayer.Pause();
            MediaPlayer.IsVisualizationEnabled = true;
            Texture2D wangTexture = TextureBank.Get("wang8");
            this.terrain = new WangMesh(
                       graphics, 
                    wangTexture, 
                             24, //cols
                            200, //rows
                              4, //tile size
                             16, //height variance
                          1E-4f, //scroll speed
            // Concavity.Inside, //orientation
              Concavity.Outside,
                             10, //depth
                            200);//radius
            
            this.playerShots = World.PlayerShots;
            this.enemyShots  = World.EnemyShots;
            this.enemyList   = World.EnemyList;
            this.gibList     = World.GibList;
            this.itemList    = World.ItemList;
            this.roster = roster;
            this.taskList = loader.Load("data/levels/0.level");
            loadMsgBoxes(content);

            this.collider = new EntityManager();
            this.physics = new PhysicsManager();
            this.camera = new Camera(100, MathHelper.PiOver2, 1);
            this.hud = new HUD();

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            //TODO parse the player file here
            player = PlayerLoader.Load("0");
        }

        private int scrollSpeed = 10;
        public void loadMsgBoxes(ContentManager content)
        {
            List<string> allLines = Util.ReadLines("data/msg_boxes.txt");
            string total = "";
            for (int i = 0; i < allLines.Count; i += 1)
            {
                string line = allLines[i].TrimEnd(' ');
                line = line + ' ';
                total += line;
            }

            string[] messages = total.Split('@');
            SpriteFont font = FontBank.Get("consolas");
            for (int i = 1; i < messages.Length; i += 1)
            {
                boxes.Add(new TextBoxList(font, messages[i], scrollSpeed));
            }
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
            World.PushContext(new Conversation(this, boxes[task.ID]));
            taskPtr += 1;
        }

        public void Process(SpawnTask task)
        {
            Enemy toSpawn = roster.Clone(task.ID, task.Args);
            enemyList.Add(toSpawn);
            taskPtr += 1;
        }

        int counter = 0;
        public void Process(WaitTask task)
        {
            if (counter >= task.Duration)
            {
                counter = 0;
                taskPtr += 1;
            }
            counter += 1;
        }

        public void ProcessTasks()
        {
            if (taskPtr >= taskList.Count) return;
            Task current = taskList[taskPtr];
            current.Process(this);
        }

        public void Update(ControlManager controls)
        {
            MediaPlayer.GetVisualizationData(soundData);
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
                collider.CollideTerrain(terrain, gib);
            }

            physics.Affect(World.DyingList, true);
            collider.CollideDying(terrain);
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


            player.Update(controls);
            player.LockPosition(camera, GameArea);


            updateEntityList(enemyList);
            updateEntityList(gibList);
            updateEntityList(itemList);
            updateEntityList(playerShots);
            updateEntityList(enemyShots);

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



            particles.SetCamera(view, projection);
            particles.Draw(graphics);

            batch.Begin();
            hud.Draw(batch, player);
            batch.End();
            
        }



        public void Load(ContentManager content)
        {

        }

        public void Unload()
        {

        }

        public void Dispose()
        {

        }
    }
}
