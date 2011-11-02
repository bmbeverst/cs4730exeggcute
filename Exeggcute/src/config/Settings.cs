using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.config
{
    static class Settings
    {
        public static GlobalConfigs Global;
        public static void Reset()
        {
            Global = new GlobalConfigs();
            //Global.WriteDefaults();
            Global.Load();
        }
    }
}
