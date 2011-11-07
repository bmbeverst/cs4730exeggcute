using System.Collections.Generic;
using Exeggcute.src.loading;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;

#pragma warning disable 0649

namespace Exeggcute.src
{
    class LightSettings : Loadable
    {
        public float? Ambient;

        public bool? DirOn;
        public Vector3? DirDirection;
        public float? DirLevel;

        public bool? Point1On;
        public Vector3? Point1Pos;
        public float? Point1Level;

        public bool? SpotOn;
        public Vector3? SpotPos;
        public Vector3? SpotDir;
        public float? SpotInner;
        public float? SpotOuter;
        public float? SpotRange;
        public float? SpotLevel;

        public LightSettings(string filename, List<string[]> tokens)
            : base(filename)
        {
            loadFromTokens(tokens, false);
            /*for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("ambient"))
                {
                    Ambient = float.Parse(value);
                }
                else if (matches("dir"))
                {
                    currentField = value;
                    if (matches("off"))
                    {
                        DirOn = false;
                        DirDirection = Vector3.Zero;
                        DirLevel = 0.0f;
                    }
                    else if (matches("on"))
                    {
                        DirOn = true;
                    }
                    else
                    {
                        throw new ParseError("a light must be either \"on\" or \"off\". Got \"{0}\" instead", value);
                    }
                }
                else if (matches("dirdirection"))
                {
                    DirDirection = Util.ParseVector3(value);
                }
                else if (matches("dirlevel"))
                {
                    DirLevel = float.Parse(value);
                }
                else if (matches("point1"))
                {
                    currentField = value;
                    if (matches("off"))
                    {
                        Point1On = false;
                        Point1Pos = Vector3.Zero;
                        Point1Level = 0.0f;
                    }
                    else if (matches("on"))
                    {
                        Point1On = true;
                    }
                    else
                    {
                        throw new ParseError("a light must be either \"on\" or \"off\". Got \"{0}\" instead", value);
                    }
                }
                else if (matches("point1pos"))
                {
                    Point1Pos = Util.ParseVector3(value);
                }
                else if (matches("point1level"))
                {
                    Point1Level = float.Parse(value);
                }
                else if (matches("spot"))
                {
                    currentField = value;
                    if (matches("off"))
                    {
                        SpotOn = false;
                        SpotPos = Vector3.Zero;
                        SpotDir = Vector3.Zero;
                        SpotInner = 0;
                        SpotOuter = 0;
                        SpotRange = 0;
                        SpotLevel = 0;
                    }
                    else if (matches("on"))
                    {
                        SpotOn = true;
                    }
                    else
                    {
                        throw new ParseError("a light must be either \"on\" or \"off\". Got \"{0}\" instead", value);
                    }
                }
                else if (matches("spotpos"))
                {
                    SpotPos = Util.ParseVector3(value);
                }
                else if (matches("spotdir"))
                {
                    SpotDir = Util.ParseVector3(value);
                }
                else if (matches("spotinner"))
                {
                    SpotInner = float.Parse(value);
                }
                else if (matches("spotouter"))
                {
                    SpotOuter = float.Parse(value);
                }
                else if (matches("spotrange"))
                {
                    SpotRange = float.Parse(value);
                }
                else if (matches("spotlevel"))
                {
                    SpotLevel = float.Parse(value);
                }
                else
                {
                    throw new ParseError("Dont know what to do with '{0}:{1}'", currentField, value);
                }
            }


            AssertInitialized(this);*/
        }
    }
}
