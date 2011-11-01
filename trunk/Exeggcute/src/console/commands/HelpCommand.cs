using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class HelpCommand : ConsoleCommand
    {
        static HelpCommand()
        {
            string baseDefaultUsage = 
@"
    Help COMMAND        Gets help about the COMMAND specified, or displays
                        this message. If COMMAND is 'all' it will display
                        the usage of all commands.

Available commands are:
";
            foreach (Keyword type in Enum.GetValues(typeof(Keyword)))
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

        public HelpCommand(DevConsole console)
            : base(console)
        {
            Output = string.Format("Usage: {0}{1}", Keyword.Help, DefaultUsage);
        }

        public HelpCommand(DevConsole console, Keyword word, string otherUsage)
            : base(console)
        {
            this.Output = string.Format("Usage: {0}{1}", word, otherUsage);
        }

        public static HelpCommand MakeTypeFailure(DevConsole devConsole, string name)
        {
            HelpCommand result = new HelpCommand(devConsole);
            result.Output = string.Format("Failed to parse command name from your input \"{0}\"\n Enter 'help' for help.", name);
            return result;
        }

        public static HelpCommand MakeUnhandled(DevConsole devConsole, Keyword type)
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

        public static HelpCommand MakeAll(DevConsole devConsole)
        {
            HelpCommand result = new HelpCommand(devConsole);
            string message = "All commands:\n";
            foreach (var pair in CommandParser.Usages)
            {
                Keyword word = pair.Key;
                if (word == Keyword.Help) continue;
                message += pair.Value + '\n';
            }
            result.Output = message;
            return result;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            devConsole.Write(Output);
        }

        
    }
}
