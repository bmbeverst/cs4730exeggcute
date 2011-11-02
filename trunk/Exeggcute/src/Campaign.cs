using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class Campaign
    {
        protected List<string> levelNames;

        public string this[int i]
        {
            get { return levelNames[i]; }
        }

        public int Count { get { return levelNames.Count; } }

        public Campaign(string name)
        {
            string filepath = string.Format("data/campaigns/{0}.campaign", name);
            levelNames = Util.ReadAndStrip(filepath, true);
        }
    }
}
