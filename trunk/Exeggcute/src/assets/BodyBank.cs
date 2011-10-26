using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.loading;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.assets
{
    class BodyBank
    {
        protected static CustomBank<BodyInfo> bank =
            new CustomBank<BodyInfo>("data/bodies");

        public static BodyInfo Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                BodyInfo info = new BodyInfo(file);
                bank.Put(info, file);
            }
        }
    }
}
