using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.console.trackers
{
    abstract class Tracker
    {
        protected Entity3D entity;
        protected string format;
        protected int[] indices;
        protected int frequency;
        protected int frame;
        protected string output = "";

        public Tracker(Entity3D entity, string format, int[] indices, int frequency)
        {
            this.entity = entity;
            this.format = format;
            this.indices = indices;
            this.frequency = frequency;
        }

        public virtual void Update()
        {
            frame += 1;
            if (frame % frequency == 0)
            {
                output = getOutput();
            }
        }
        protected virtual string getOutput()
        {
            return entity.FormattedQuery(format, indices);
        }

        public abstract void Draw2D(SpriteBatch batch, Vector2 pos);
    }
}
