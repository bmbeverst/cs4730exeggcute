using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class GibInfo : Loadable
    {
        protected Model model;
        protected Texture2D texture;
        protected float? scale;

        public GibInfo(string name)
        {
            string filename = string.Format("data/bodies/{0}.body", name);
            loadFromFile(filename);
        }

        public Gib MakeGib()
        {
            return new Gib(model, texture, scale.Value);
        }
    }
}
