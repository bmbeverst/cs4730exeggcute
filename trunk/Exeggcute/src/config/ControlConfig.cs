using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.config
{
    class ControlConfig : Config
    {

        private static Dictionary<string, string> DEFAULTS;
        static ControlConfig()
        {
            DEFAULTS = new Dictionary<string, string>
            {
                {"Up",       "W,LeftThumbstickUp"},
                {"Down",     "S,LeftThumbstickDown"},
                {"Left",     "A,LeftThumbstickLeft"},
                {"Right",    "D,LeftThumbstickRight"},
                {"Start",    "Enter,Start"},
                {"Select",   "Space,RightTrigger"},
                {"Action",   "N,A"},
                {"Cancel",   "M,X"},
                {"Focus",    "Space,Y"},
                {"LShoulder","H,LeftShoulder"},
                {"RShoulder","J,RightShoulder"},
                {"Skip",     "LeftShift,X"},
                {"Quit",     "Escape,RightThumbstickLeft"},
                {"Console",  "OemTilde,RightThumbstickRight"}
            };
        }

        public override Dictionary<string, string> GetDefault()
        {
            return DEFAULTS;
        }

        public override void Apply()
        {

        }

        public override void Set(string name, string value)
        {

        }
    }
}
