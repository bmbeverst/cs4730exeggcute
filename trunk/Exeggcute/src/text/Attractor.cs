using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.text
{
    class Attractor
    {
        public Vector3 Position { get; protected set; }
        public float Force { get; protected set; }
        public int DecayExp { get; protected set; }
        public float DeadzoneRadius { get; protected set; }
    }
}
