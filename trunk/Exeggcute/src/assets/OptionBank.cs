using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.loading;

namespace Exeggcute.src.assets
{
    class OptionBank
    {
        protected static CustomBank<OptionInfo> bank =
            new CustomBank<OptionInfo>("data/options");

        public static OptionInfo Get(string name)
        {
            return bank[name];
        }
        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                OptionInfo info = new OptionInfo(file);
                bank.Put(info, file);
            }
        }
    }
}
