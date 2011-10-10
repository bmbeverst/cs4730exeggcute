using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    abstract class IAnimation
    {
        public virtual Point CurrentFrame { get; protected set; }
        public virtual Texture2D Texture { get; protected set; }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }
        public abstract void Update();
        public abstract void ChangeSpeed(int newSpeed);
        public virtual Rectangle GetFrameRect()
        {
            return new Rectangle(CurrentFrame.X, CurrentFrame.Y, Width, Height);
        }

    }
}
