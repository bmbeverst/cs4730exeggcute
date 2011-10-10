using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    /// <summary>
    /// Contains a list of pointers to locations on a Texture2D. The list
    /// is scrolled through at a fixed rate in a specified order.
    /// </summary>
    class Animation : IAnimation
    {
        private Point[] frames;
        private int[] order;
        private int orderPtr;
        private Timer frameTimer;
        private bool loop;
        private int loopAt;
        public override Point CurrentFrame
        {
            get { return frames[order[orderPtr]]; }
            protected set { throw new Exception("dont touch me"); }
        }
        public Animation(TextureName texName, int width, int height, Point[] frames, int[] order, int speed, bool loop, int loopAt)
        {
            this.Texture = TextureBank.Get(texName);
            this.Width = width;
            this.Height = height;
            this.frames = frames;
            this.order = order;
            this.frameTimer = new Timer(speed);
            this.loop = loop;
            this.loopAt = loopAt;
            this.orderPtr = 0;
        }

        public override void Update()
        {
            frameTimer.Increment();
            if (frameTimer.IsDone)
            {
                frameTimer.Reset();
                if (loop) orderPtr = Math.Max((orderPtr + 1) % order.Length, loopAt);
                else  orderPtr = Math.Max(orderPtr + 1, order.Length);
            }
        }

        public override void ChangeSpeed(int newSpeed)
        {
            frameTimer.ChangeMax(newSpeed);
        }

    }
}
