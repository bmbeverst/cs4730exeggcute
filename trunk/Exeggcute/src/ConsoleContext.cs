using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    /// <summary>
    /// A context which can have a console window held over it.
    /// </summary>
    abstract class ConsoleContext : IContext
    {
        public IContext Parent { get; protected set;  }
        
        public abstract void Update(ControlManager controls);
        public abstract void Draw2D(SpriteBatch batch);
        public abstract void Draw3D(GraphicsDevice graphics, Camera camera);
        public abstract void Unload();
        public abstract void Dispose();

        public virtual void AcceptCommand(ConsoleCommand command)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void AcceptCommand(ContextCommand context)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void AcceptCommand(HelpCommand help)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void AcceptCommand(SpawnCommand spawn)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void AcceptCommand(ListCommand list)
        {
            throw new SubclassShouldImplementError();
        }
    }
}
