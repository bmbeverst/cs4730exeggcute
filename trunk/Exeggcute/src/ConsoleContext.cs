using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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

        protected VisualizationData soundData = new VisualizationData();

        public virtual void AcceptCommand(ConsoleCommand command)
        {
            SubclassShouldImplement(command);
        }


        public virtual void AcceptCommand(ReloadCommand reload)
        {
            SubclassShouldImplement(reload);
        }

        public virtual void AcceptCommand(SetGlobalCommand global)
        {
            SubclassShouldImplement(global);
        }

        public virtual void AcceptCommand(BuiltinCommand cmd)
        {
            SubclassShouldImplement(cmd);
        }

        public virtual void AcceptCommand(ClearCommand clear)
        {
            SubclassShouldImplement(clear);
        }

        public virtual void AcceptCommand(TrackCommand track)
        {
            SubclassShouldImplement(track);
        }

        public virtual void AcceptCommand(SetParamCommand set)
        {
            SubclassShouldImplement(set);
        }

        public virtual void AcceptCommand(WhatIsCommand whatis)
        {
            SubclassShouldImplement(whatis);
        }

        public virtual void AcceptCommand(DocCommand doc)
        {
            SubclassShouldImplement(doc);
        }

        public virtual void AcceptCommand(LevelTaskCommand task)
        {
            SubclassShouldImplement(task);
        }

        public virtual void AcceptCommand(GoCommand context)
        {
            SubclassShouldImplement(context);
        }

        public virtual void AcceptCommand(HelpCommand help)
        {
            SubclassShouldImplement(help);
        }

        public virtual void AcceptCommand(SpawnCommand spawn)
        {
            SubclassShouldImplement(spawn);
        }

        public virtual void AcceptCommand(ListCommand list)
        {
            SubclassShouldImplement(list);
        }

        public virtual void AcceptCommand(LoadSetCommand restore)
        {
            SubclassShouldImplement(restore);
        }

        public virtual void AcceptCommand(PackageCommand package)
        {
            SubclassShouldImplement(package);
        }

        public virtual void AcceptCommand(ResetCommand reset)
        {
            SubclassShouldImplement(reset);
        }

        public virtual void AcceptCommand(ExitCommand exit)
        {
            SubclassShouldImplement(exit);
        }

        protected void SubclassShouldImplement(ConsoleCommand command)
        {
            command.devConsole.WriteLine("A subclass should implement a handler for {0}", command.GetType().Name);
        }
    }
}
