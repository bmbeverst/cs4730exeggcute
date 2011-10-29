using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;
using Exeggcute.src.scripting;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    /// <summary>
    /// Storage class for loading in a WangMesh off the disk.
    /// </summary>
    class TerrainInfo : Loadable
    {
        public int? Depth;
        public int? Radius;
        public int? Cols;
        public int? Rows;
        public float? TileSize;
        public float? ScrollSpeed;
        public float? Variance;
        public Texture2D Texture;
        public Concavity? Orientation;
        public TerrainType? Type;

        public TerrainInfo(string filename)
            : base(filename)
        {
            loadFromFile(filename);
        }

        public TerrainInfo(string filename, List<string[]> lines)
            : base(filename)
        {
            this.loadFromTokens(lines);
        }

        public WangMesh MakeMesh(GraphicsDevice graphics)
        {
            return new WangMesh(graphics,
                                Texture,
                                Cols.Value,
                                Rows.Value,
                                TileSize.Value,
                                Variance.GetValueOrDefault(),
                                ScrollSpeed.Value,
                                Orientation.Value,
                                Type.Value,
                                Depth.Value,
                                Radius.Value);
        }
    }
}
