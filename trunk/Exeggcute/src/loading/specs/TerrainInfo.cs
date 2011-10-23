using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;

namespace Exeggcute.src.loading.specs
{

    class TerrainInfo
    {
        public int Depth { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public float ScrollDirection { get; protected set; }
        public Concavity Orientation { get; protected set; }
        public int Width { get; protected set; }
        public float Radius { get; protected set; }


    }
}
