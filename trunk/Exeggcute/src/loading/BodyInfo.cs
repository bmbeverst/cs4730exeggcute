using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class BodyInfo : Loadable
    {
        public Model Model;
        public Texture2D Texture;
        public float? Scale;

        public BodyInfo(string filename)
        {
            loadFromFile(filename);
        }

        public static BodyInfo Parse(string s)
        {
            return BodyBank.Get(s);
        }
    }
}
