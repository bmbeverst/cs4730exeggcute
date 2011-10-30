using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;

namespace Exeggcute.src.console.commands
{
    class HelpCommand : ConsoleCommand
    {
        static HelpCommand()
        {
            string baseDefaultUsage = 
@"Usage: Help 
    Help COMMAND        Gets help about the COMMAND specified, or displays
                        this message. 

Available commands are:
";
            foreach (ConsoleCommandType type in Enum.GetValues(typeof(ConsoleCommandType)))
            {
                baseDefaultUsage += type.ToString() + "\n";
            }

            DefaultUsage = baseDefaultUsage;
        }

        /// <summary>
        /// the message displayed for "help help"
        /// </summary>
        public static string Usage = 
@"
You have successfully used the help command to ask for help on using the help
command. Thank you for helping us help you help us all.
";
        /// <summary>
        /// the message displayed for "help"
        /// </summary>
        public static string DefaultUsage = ConsoleCommand.StaticUsage;

        public string Name { get; protected set; }

        public string Output { get; protected set; }

        public HelpCommand(DevConsole console, string otherUsage)
            : base(console)
        {
            string msg;
            if (otherUsage == null)
            {
                msg = DefaultUsage;
                
            }
            else
            {
                msg = otherUsage;
            }
            string message = string.Format("{0}", msg);
            Output = message;

        }

        private HelpCommand(DevConsole devConsole)
            : base(devConsole)
        {

        }

        public static HelpCommand MakeTypeFailure(DevConsole devConsole, string name)
        {
            HelpCommand result = new HelpCommand(devConsole);
            result.Output = string.Format("Failed to parse typename from your input \"{0}\".", name);
            return result;
        }

        public static HelpCommand MakeUnhandled(DevConsole devConsole, ConsoleCommandType type)
        {
            HelpCommand result = new HelpCommand(devConsole);
            result.Output = string.Format("The parser for the type \"{0}\" has not yet been implemented.", type);
            return result;
        }

        public static HelpCommand MakeGeneric(DevConsole devConsole, string usage)
        {
            HelpCommand result = new HelpCommand(devConsole);
            result.Output = usage;
            return result;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            devConsole.Write(Output);
        }

        
    }
}
