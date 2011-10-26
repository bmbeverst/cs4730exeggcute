using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                GibInfo entry = new GibInfo(line);
                gibs.Add(entry.MakeGib());
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
