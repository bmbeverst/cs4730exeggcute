using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.console.trackers
{
    class ConsoleTracker : Tracker
    {
        public ConsoleTracker(Entity3D entity, string format, int[] indices, int frequency)
            : base(entity, format, indices, frequency)
        {

        }
        public override void Update()
        {
            base.Update();
            Console.WriteLine(output);
        }

        public override void Draw2D(SpriteBatch batch, Vector2 pos)
        {

        }
    }
}
