using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class WhatIsCommand : ConsoleCommand
    {
        private static Dictionary<string, string> documentation;
        static WhatIsCommand()
        {
            documentation = new Dictionary<string, string>
            {
                { "floatvalue", "A FloatValue is either a float literal, or a FloatRange." },
                { "floatrange", 
@"A FloatRange denotes a random value in an interval. 
Syntax: [float|float]
    where float is any number accepted by float.Parse(string).
Example: [-5|5]
    designates a float between -5 and 5.
Note that the left hand side must be less than or equal to the right hand side." },
                { "float3", 
@"A Float3 is a 3-tuple of FloatValues.
Syntax: (FloatValue,FloatValue,FloatValue)
    where the entries correspond to X, Y, and Z respectively (usually).
Example: (0,0,[0|99])" }
            };
        }
        public static string Usage =
@"    
    whatis TYPE         Gives a description and syntax for the given type.";

        public string Type { get; protected set; }

        public WhatIsCommand(DevConsole devConsole, string type)
            : base(devConsole)
        {
            this.Type = type;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            devConsole.WriteLine(documentation[Type.ToLower()]);
        }
    }
}
