using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class Campaign
    {
        public List<string> LevelNames { get; protected set; }

        public Campaign(string name)
        {
            string filepath = string.Format("data/campaigns/{0}.campaigns", name);

        }
    }
}
