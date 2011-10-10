using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.input;

namespace Exeggcute.src
{
    class ShotSpawner
    {
        private Timer timer;
        private Shot shot;
        private float angle;
        private float speed;
        private Vector3 offset;

        public ShotSpawner(Shot shot, Vector2 offset, int cooldown, int cdoffset, float angle, float speed)
        {
            this.shot = shot;
            this.offset = new Vector3(offset.X, offset.Y, 0);
            timer = new Timer(cooldown,cooldown - cdoffset);
            this.angle = angle;
            this.speed = speed;
        }

        public Shot TrySpawnAt(Vector3 pos, Keyflag flag)
        {
            if (flag.IsPressed)
            {
                timer.Increment();
                if (timer.IsDone)
                {
                    timer.Reset();
                    return shot.Clone(pos + offset, angle, speed);
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
