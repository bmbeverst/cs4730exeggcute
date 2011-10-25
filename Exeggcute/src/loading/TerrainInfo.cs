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
    /// <summary>
    /// Storage class for loading in a WangMesh off the disk.
    /// </summary>
    class TerrainInfo : LoadedInfo
    {
        public int? Depth { get; protected set; }
        public int? Radius { get; protected set; }
        public int? Cols { get; protected set; }
        public int? Rows { get; protected set; }
        public float? TileSize { get; protected set; }
        public float? ScrollSpeed { get; protected set; }
        public float? HeightVariance { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public Concavity? Orientation { get; protected set; }
        public TerrainType? Type { get; protected set; }

        protected TerrainInfo(GraphicsDevice graphics, List<string[]> data)
        {
            for (int i = 0; i < data.Count; i += 1)
            {
                string[] tokens = data[i];
                currentField = tokens[0];
                string value = tokens[1];

                if (matches("depth"))
                {
                    Depth = int.Parse(value);
                }
                else if (matches("radius"))
                {
                    Radius = int.Parse(value);
                }
                else if (matches("rows"))
                {
                    Rows = int.Parse(value);
                }
                else if (matches("cols"))
                {
                    Cols = int.Parse(value);
                }
                else if (matches("TileSize"))
                {
                    TileSize = float.Parse(value);
                }
                else if (matches("ScrollSpeed"))
                {
                    ScrollSpeed = float.Parse(value);
                }
                else if (matches("variance"))
                {
                    Type = TerrainType.Varied;
                    HeightVariance = float.Parse(value);
                }
                else if (matches("type"))
                {
                    Type = Util.ParseEnum<TerrainType>(value);
                    if (Type != TerrainType.Varied)
                    {
                        HeightVariance = 0;
                    }
                }
                else if (matches("Texture"))
                {
                    Texture = TextureBank.Get(value);
                }
                else if (matches("Orientation"))
                {
                    Orientation = Util.ParseEnum<Concavity>(value);
                }
                else
                {
                    throw new ParseError("No known field with that name \"{0}\"", currentField);
                }

            }

            AssertInitialized(this);
        }
        public static WangMesh Make(GraphicsDevice graphics, List<string[]> data)
        {
            TerrainInfo info = new TerrainInfo(graphics, data);
            return info.load(graphics);
        }
       protected WangMesh load(GraphicsDevice graphics)
        {
            
            return new WangMesh(graphics,
                                Texture,
                                Cols.Value,
                                Rows.Value,
                                TileSize.Value,
                                HeightVariance.Value,
                                ScrollSpeed.Value,
                                Orientation.Value, 
                                Type.Value, 
                                Depth.Value, 
                                Radius.Value);
        }
    }
}
