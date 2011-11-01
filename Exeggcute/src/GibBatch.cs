using System.Collections.Generic;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.loading;

namespace Exeggcute.src
{
    class GibBatch
    {
        public List<Gib> gibs = new List<Gib>();
        public GibBatch(string name)
        {
            string filepath = string.Format("data/gibs/{0}.gib", name);
            List<string> lines = Util.ReadAndStrip(filepath, true);
            foreach (string line in lines)
            {
                BodyInfo body = Assets.Body[line];
                Gib gib = new Gib(body.Model, body.Texture, body.Scale.Value, body.Radius.Value, body.Rotation.Value);
                gibs.Add(gib);
            }
        }

        public List<Gib> Clone()
        {
            List<Gib> copy = new List<Gib>();
            foreach (Gib g in gibs)
            {
                copy.Add(g.Copy());
            }
            return copy;
        }

        public static GibBatch Parse(string s)
        {
            return new GibBatch(s);
        }


    }
}
