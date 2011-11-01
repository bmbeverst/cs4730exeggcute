using System.Collections.Generic;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;

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
        public float Variance = 0;
        public Texture2D Texture;
        public Concavity? Orientation;
        public TerrainType? Type;

        public TerrainInfo(string filename)
            : base(filename)
        {
            loadFromFile(filename, true);
        }

        public TerrainInfo(string filename, List<string[]> lines)
            : base(filename)
        {
            this.loadFromTokens(lines, true);
        }

        public WangMesh MakeMesh(GraphicsDevice graphics)
        {
            return new WangMesh(graphics,
                                Texture,
                                Cols.Value,
                                Rows.Value,
                                TileSize.Value,
                                Variance,
                                ScrollSpeed.Value,
                                Orientation.Value,
                                Type.Value,
                                Depth.Value,
                                Radius.Value);
        }
    }
}
