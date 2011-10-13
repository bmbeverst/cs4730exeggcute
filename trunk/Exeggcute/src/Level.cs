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

        private HashList<Shot> playerShots = new HashList<Shot>();
        private HashList<Shot> enemyShots = new HashList<Shot>();

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

            updateShots(playerShots);
            updateShots(enemyShots);
            player.Update(controls);
            player.LockPosition(camera, GameArea);

            entities.ForEach(e => e.Update());
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

            player.Draw(graphics, view, projection);

            drawShots(playerShots, graphics, view, projection);
            drawShots(enemyShots, graphics, view, projection);
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
