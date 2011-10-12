﻿using System;
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
        public HUD hud;
        private ParticleSystem particles;
        private Camera camera;
        private Player3D player;
        private CollisionManager collider;
        private List<Entity3D> entities = new List<Entity3D>();

        public Level(GraphicsDevice device, ContentManager content)
        {
            collider = new CollisionManager();
            camera = new Camera(100, MathHelper.PiOver2, 1);
            hud = new HUD();
            particles = new TestParticleSystem(device, content);
            player = new Player3D(ModelName.testcube, new Vector3(-20,0,0));
            entities.Add(new CommandEntity(ModelName.testcube));
        }

        public void Update(ControlManager controls)
        {
            particles.Update();
            for (int i = 0; i < 1; i += 1)
            {
                if (player.Velocity.Equals(Vector3.Zero)) break;
                particles.AddParticle(player.Position, -5*player.Velocity);
            }
            collider.Collide(player, entities);
            player.Update(controls);
            player.LockPosition(camera);

            entities.ForEach(e => e.Update());
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();

            player.Draw(graphics, view, projection);
            particles.SetCamera(view, projection);
            particles.Draw(graphics);
            entities.ForEach(e => e.Draw(graphics, view, projection));
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
