using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.console;
using Exeggcute.src.console.commands;

namespace Exeggcute.src
{
    class Sandbox : ConsoleContext
    {

        public EntityManager collider;

        public Sandbox()
        {
            collider = new EntityManager();
        }


        public override void AcceptCommand(ConsoleCommand command)
        {

        }

        public override void AcceptCommand(ContextCommand context)
        {
            //if (context.
        }
        
        public override void Update(ControlManager controls)
        {
            
        }


        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {

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
