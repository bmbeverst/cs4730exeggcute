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
        private Player player;
        private CollisionManager collider;
        private PhysicsManager physics;
        private Roster roster;
        private WangMesh terrain;

        private HashList<Enemy> enemyList;

        private HashList<Shot> playerShots;
        private HashList<Shot> enemyShots;
        private HashList<Gib> gibList;

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

        //FIXME put a lot of this stuff in Load!
        public Level(GraphicsDevice graphics, ContentManager content, RosterName rosterName)
        {
            this.terrain = new WangMesh(graphics, TextureName.wang8, 12*2, 100*2, 4, 16, 0.01f);
            this.playerShots = World.PlayerShots;
            this.enemyShots = World.EnemyShots;
            this.enemyList = World.EnemyList;
            this.gibList = World.GibList;
            this.roster = RosterBank.Get(rosterName);
            this.taskList = loader.Load(0);
            loadMsgBoxes(content);

            this.collider = new CollisionManager();
            this.physics = new PhysicsManager();
            this.camera = new Camera(100, MathHelper.PiOver2, 1);
            this.hud = new HUD();

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            player = new Player(ModelName.playerScene, ArsenalName.test, World.PlayerShots, World.GibList);
            
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
            SpriteFont font = FontBank.Get(FontName.font0);
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
                foreach (var pair in shots)
                {
                    Shot current = pair.Key;
                    current.Update();
                    if (!current.ContainedIn(LiveArea))
                    {
                        toRemove.Add(current);
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
            //camera.Update(controls);
            ProcessTasks();
            particles.Update();
            //terrain.Update();
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
            bool hit = collider.Collide(player, enemyList);
            if (hit)
            {
                player.Kill();
            }
            collider.Collide(playerShots, enemyList);
            
            // =[
            collider.FilterDead(playerShots);
            collider.FilterDead<Shot>(enemyShots);
            collider.FilterDead<Gib>(gibList);
            collider.FilterDead<Enemy>(enemyList);
            collider.FilterDead<Enemy>(World.DyingList);
            updateShots(playerShots, enemyShots);

            player.Update(controls);
            player.LockPosition(camera, GameArea);

            foreach (Enemy enemy in enemyList.GetKeys())
            {
                enemy.Update();
            }

            foreach (Gib gib in gibList.GetKeys())
            {
                gib.Update();
            }
        }

        private void drawShots(GraphicsDevice graphics, Matrix view, Matrix projection, params HashList<Shot>[] shotLists )
        {
            foreach (HashList<Shot> shots in shotLists)
            {
                foreach (Shot shot in shots.GetKeys())
                {
                    shot.Draw(graphics, view, projection);
                }
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            terrain.Draw(graphics, view, projection);

            player.Draw(graphics, view, projection);

            drawShots(graphics, view, projection,playerShots, enemyShots);

            foreach (Enemy enemy in enemyList.GetKeys())
            {
                enemy.Draw(graphics, view, projection);
            }

            foreach (Gib gib in gibList.GetKeys())
            {
                gib.Draw(graphics, view, projection);
            }

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
