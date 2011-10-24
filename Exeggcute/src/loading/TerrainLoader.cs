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
    class TerrainLoader
    {
        public WangMesh Load(GraphicsDevice graphics, List<string[]> data)
        {
            int? Depth = null;
            int? Radius = null;
            int? Cols = null;
            int? Rows = null;
            float? TileSize = null;
            float? ScrollSpeed = null;
            Texture2D Texture = null;
            Concavity? Orientation = null;

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

            if (Depth == null ||
                Radius == null ||
                Cols == null ||
                Rows == null ||
                TileSize == null ||
                ScrollSpeed == null ||
                Texture == null ||
                Orientation == null)
            {
                throw new ParseError("not all fields were initialized");
            }
            currentField = null;
            return new WangMesh(graphics, Texture, Cols.Value, Rows.Value, TileSize.Value, 0, ScrollSpeed.Value, Orientation.Value, Depth.Value, Radius.Value);
        }

        protected static string currentField;

        protected static bool matches(string regex)
        {
            return Regex.IsMatch(currentField, regex, RegexOptions.IgnoreCase);
        }


    }
}
