using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;
using Exeggcute.src.scripting;

namespace Exeggcute.src.console
{
    class CommandParser
    {
        public ConsoleCommand Parse(DevConsole console, string input)
        {
            string[] tokens = input.Split(' ');
            string typeName = tokens[0];
            ConsoleCommandType type;
            try
            {
                type = Util.ParseEnum<ConsoleCommandType>(typeName);
            }
            catch
            {
                return HelpCommand.MakeTypeFailure(console, typeName);
            }

            ConsoleCommand command;

            if (type == ConsoleCommandType.Context)
            {
                command = new ContextCommand(console, tokens[1]);
            }
            else if (type == ConsoleCommandType.Help)
            {
                string otherUsage = tokens.Length > 1 ? GetUsage(tokens[1]) : null;
                command = new HelpCommand(console, otherUsage);

            }
            else if (type == ConsoleCommandType.List)
            {
                FileType fileType;
                string typeString = tokens[1];
                try
                {
                    fileType = Util.ParseEnum<FileType>(typeString);
                }
                catch
                {
                    string msg = string.Format("No FileType with name \"{0}\"\n{1}", typeString, ListCommand.Usage);
                    return HelpCommand.MakeGeneric(console, msg);
                }
                command = new ListCommand(console, fileType);
            }
            else
            {
                return HelpCommand.MakeUnhandled(console, type);
            }


            return command;
        }

        public string GetUsage(string name)
        {
            ConsoleCommandType type;
            try
            {
                type = Util.ParseEnum<ConsoleCommandType>(name);
            }
            catch
            {
                return string.Format("I don't know any command named {0}.\n{1}", name, HelpCommand.DefaultUsage);
            }

            if (type == ConsoleCommandType.Context)
            {
                return ContextCommand.Usage;
            }
            else if (type == ConsoleCommandType.Help)
            {
                return HelpCommand.Usage;
            }
            else if (type == ConsoleCommandType.Spawn)
            {
                return SpawnCommand.Usage;
            }
            else
            {
                return string.Format("\n    Usage message for {0} not implemented", type);
            }
        }

        
    }
}
