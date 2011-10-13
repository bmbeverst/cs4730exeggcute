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
        private List<CommandEntity> entities = new List<CommandEntity>();

        private List<Shot> playerShots = new List<Shot>();
        private List<Shot> enemyShots = new List<Shot>();

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

        //FIXME put a lot of this stuff in Load!
        public Level(GraphicsDevice graphics, ContentManager content)
        {
            collider = new CollisionManager();
            camera = new Camera(100, MathHelper.PiOver2, 1);
            hud = new HUD();

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            player = new Player(ModelName.testcube, playerShots);
            List<Shot> spawnList = new List<Shot> {
                new Shot(ModelName.testcube, ScriptName.playershot0, enemyShots)
            };
            entities.Add(new CommandEntity(ModelName.testcube, ScriptName.test, spawnList, enemyShots));
        }

        private void filterShots(List<Shot> list)
        {
            for (int i = list.Count - 1; i >= 0; i -= 1)
            {
                if (!list[i].ContainedIn(LiveArea))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public void Update(ControlManager controls)
        {
            particles.Update();
            for (int i = 0; i < 1; i += 1)
            {
                if (player.Velocity.Equals(Vector3.Zero)) break;
                particles.AddParticle(player.Position, -5*player.Velocity);
            }

            bool hit = collider.Collide(player, entities);
            if (hit)
            {
                player.Kill();
            }

            filterShots(playerShots);
            filterShots(enemyShots);
            player.Update(controls);
            player.LockPosition(camera, GameArea);

            playerShots.ForEach(shot => shot.Update());
            enemyShots.ForEach(shot => shot.Update());

            entities.ForEach(e => e.Update());
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();

            player.Draw(graphics, view, projection);

            playerShots.ForEach(shot => shot.Draw(graphics, view, projection));
            enemyShots.ForEach(shot => shot.Draw(graphics, view, projection));

            entities.ForEach(e => e.Draw(graphics, view, projection));

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
