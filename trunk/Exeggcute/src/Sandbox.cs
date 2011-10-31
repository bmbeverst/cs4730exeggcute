using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.console;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class Sandbox : ConsoleContext
    {
        public EntityManager collider;
        public Player Player;
        public Rectangle GameArea;
        public Rectangle LiveArea;

        protected float liveBuffer = 1f / 4f;
        public Sandbox()
        {
            collider = new EntityManager();
            GameArea = new Rectangle(-Level.HalfWidth, -Level.HalfHeight, Level.HalfWidth * 2, Level.HalfHeight * 2);
            LiveArea = Util.GrowRect(GameArea, liveBuffer);
        }

        public override void AcceptCommand(ConsoleCommand command)
        {

        }

        public override void AcceptCommand(GoCommand context)
        {
            //if (context.
        }
        
        public override void Update(ControlManager controls)
        {
            if (Player != null)
            {
                Player.Update(controls, true);
                Player.LockPosition(GameArea);
            }
            collider.UpdateAll(LiveArea);
        }


        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            if (Player != null)
            {
                Player.Draw(graphics, view, projection);
            }

            collider.DrawAll(graphics, projection, view);
        }


        public override void Draw2D(SpriteBatch batch)
        {

        }

        public override void Unload()
        {

        }


        public override void Dispose()
        {

        }
    }
}
