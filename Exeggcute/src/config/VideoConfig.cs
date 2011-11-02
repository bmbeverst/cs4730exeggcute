using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;

namespace Exeggcute.src.config
{
    class VideoConfig : Config
    {
        private static readonly Dictionary<string, string> DEFAULT =
            new Dictionary<string,string>
            {
                {"XRes","1200"},
                {"YRes","900"},
                {"Fullscreen", "true" }
            };

        public override Dictionary<string, string> GetDefault()
        {
            return DEFAULT;
        }

        private List<Point> validRes = new List<Point>
        {
            new Point(1200,900)

        };

        public int XRes { get; protected set; }
        public int YRes { get; protected set; }
        public bool Fullscreen { get; protected set; }

        public override void Apply()
        {
            if (!validRes.Contains(new Point(XRes, YRes)))
            {
                //the bad place
            }
        }

        public override void Set(string name, string value)
        {
            Matcher match = new Matcher(name);
            if (match["xres"])
            {
                XRes = int.Parse(value);
            }
            else if (match["yres"])
            {
                YRes = int.Parse(value);
            }
            else if (match["fullscreen"])
            {
                Fullscreen = bool.Parse(value);
            }
            else
            {
                throw new ParseError("Failed to initialize {0}, no such property", name);
            }
        }
    }
}
