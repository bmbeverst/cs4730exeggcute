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
        private Roster roster;
        private WangMesh terrain;

        private HashList<Enemy> enemies = new HashList<Enemy>();

        private HashList<Shot> playerShots = new HashList<Shot>();
        private HashList<Shot> enemyShots = new HashList<Shot>();
        private HashList<Shot> gibList = new HashList<Shot>();

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
            this.terrain = new WangMesh(graphics, TextureName.wang8, 12, 100, 8, 64, 0.01f);
            
            this.playerShots = World.PlayerShots;
            this.enemyShots = World.EnemyShots;
            this.roster = RosterBank.Get(rosterName);
            this.taskList = loader.Load(0);
            loadMsgBoxes(content);

            collider = new CollisionManager();
            camera = new Camera(100, MathHelper.PiOver2, 1);
            hud = new HUD();

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            player = new Player(ModelName.playerScene, ArsenalName.test, World.PlayerShots);
            ;
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

        private void updateShots(HashList<Shot> list)
        {
            List<Shot> toRemove = new List<Shot>();
            foreach (var pair in list)
            {
                Shot current = pair.Key;
                current.Update();
                if (!current.ContainedIn(LiveArea))
                {
                    toRemove.Add(current);
                }
            }
            foreach (Shot shot in toRemove)
            {
                list.Remove(shot);
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
            enemies.Add(toSpawn);
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
            for (int i = 0; i < 1; i += 1)
            {
                if (player.Velocity.Equals(Vector3.Zero)) break;
                particles.AddParticle(player.Position, -5*player.Velocity);
            }

            bool hit = collider.Collide(player, enemies);
            if (hit)
            {
                player.Kill();
            }

            collider.Collide(playerShots, enemies);
            updateShots(playerShots);
            updateShots(enemyShots);
            player.Update(controls);
            player.LockPosition(camera, GameArea);

            foreach (Enemy enemy in enemies.GetKeys())
            {
                enemy.Update();
            }
        }

        private void drawShots(HashList<Shot> shots, GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            foreach (Shot shot in shots.GetKeys())
            {
                shot.Draw(graphics, view, projection);
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            terrain.Draw(graphics, view, projection);
            player.Draw(graphics, view, projection);

            drawShots(playerShots, graphics, view, projection);
            drawShots(enemyShots, graphics, view, projection);

            foreach (Enemy enemy in enemies.GetKeys())
            {
                enemy.Draw(graphics, view, projection);
            }

            particles.SetCamera(view, projection);
            particles.Draw(graphics);
            hud.Draw(batch, player);

            
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
