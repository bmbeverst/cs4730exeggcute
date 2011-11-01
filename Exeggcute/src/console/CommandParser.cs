using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;

namespace Exeggcute.src.console
{
    class CommandParser
    {
        public static Dictionary<Keyword, string> Usages { get; protected set; }

        public CommandParser()
        {
            Usages = new Dictionary<Keyword, string> {
                { Keyword.Help, HelpCommand.Usage },
                { Keyword.Go, GoCommand.Usage },
                { Keyword.Spawn, SpawnCommand.Usage },
                { Keyword.LoadSet, LoadSetCommand.Usage },
                { Keyword.Reload, ReloadCommand.Usage },
                { Keyword.LevelTask, LevelTaskCommand.Usage },
                { Keyword.Package, PackageCommand.Usage },
                { Keyword.List, ListCommand.Usage },
                { Keyword.Reset, ResetCommand.Usage },
                { Keyword.Exit, ExitCommand.Usage },
                { Keyword.Doc, DocCommand.Usage }
            };
        }

        public ConsoleCommand Parse(DevConsole console, string input)
        {
            string[] tokens = input.Split(' ');
            string commandTypeString = tokens[0];
            Keyword type;
            try
            {
                type = Util.ParseEnum<Keyword>(commandTypeString);
            }
            catch
            {
                return HelpCommand.MakeTypeFailure(console, commandTypeString);
            }

            ConsoleCommand command;
            try
            {
                if (type == Keyword.Go)
                {
                    command = new GoCommand(console, tokens[1]);
                }
                else if (type == Keyword.Help)
                {
                    if (tokens.Length > 1 && !Util.IsWhitespace(tokens[1]))
                    {
                        if (Util.StrEq(tokens[1], "all"))
                        {
                            return HelpCommand.MakeAll(console);
                        }
                        try
                        {
                            Keyword otherType = Util.ParseEnum<Keyword>(tokens[1]);
                            return new HelpCommand(console, otherType, GetUsage(otherType));
                        }
                        catch
                        {
                            return HelpCommand.MakeGeneric(console, "uh");
                        }
                    }
                    else
                    {
                        return new HelpCommand(console);
                    }
                    
                }
                else if (type == Keyword.List)
                {
                    FileType fileType;
                    string typeString = tokens[1];
                    try
                    {
                        fileType = Util.ParseEnum<FileType>(typeString);
                    }
                    catch
                    {
                        string msg = string.Format("No FileType with name \"{0}\". Permissable values include:\n{1}", typeString, ListCommand.ValidTypes);
                        return HelpCommand.MakeGeneric(console, msg);
                    }
                    command = new ListCommand(console, fileType);
                }
                else if (type == Keyword.Spawn)
                {
                    SpawnType spawnType;
                    string typeString = tokens[1];
                    string name = tokens[2];
                    string posString;
                    string angleString;
                    try
                    {
                        spawnType = Util.ParseEnum<SpawnType>(typeString);
                    }
                    catch
                    {
                        string msg = string.Format("No SpawnType with name \"{0}\"\n{1}", typeString, SpawnCommand.Usage);
                        return HelpCommand.MakeGeneric(console, msg);
                    }

                    try
                    {
                        posString = tokens[3];
                        angleString = tokens[4];
                    }
                    catch
                    {
                        console.Write("Got incomplete enemy arguments, using defaults");
                        posString = "(0,0,0)";
                        angleString = "0";
                    }

                    Float3 pos;
                    FloatValue angle;
                    try
                    {
                        pos = Float3.Parse(posString);
                        angle = FloatValue.Parse(angleString);
                    }
                    catch
                    {
                        string msg = string.Format("Syntax error on spawn arguments ({0} {1})", posString, angleString);
                        return HelpCommand.MakeGeneric(console, msg);
                    }
                    command = new SpawnCommand(console, spawnType, name, pos, angle);
                }
                else if (type == Keyword.LoadSet)
                {
                    string name = tokens[1];
                    command = new LoadSetCommand(console, name);
                }
                else if (type == Keyword.Package)
                {
                    string name = tokens[1];
                    command = new PackageCommand(console, name);
                }
                else if (type == Keyword.Reset)
                {
                    command = new ResetCommand(console);
                }
                else if (type == Keyword.Exit)
                {
                    command = new ExitCommand(console);
                }
                else if (type == Keyword.LevelTask)
                {
                    List<string> toks = tokens.ToList();
                    toks.RemoveAt(0);
                    string taskString = Util.Join(toks, ' ');
                    command = new LevelTaskCommand(console, taskString);
                }
                else if (type == Keyword.Doc)
                {
                    string typeString = tokens[1];

                    string filename = tokens.Length >= 3 ? tokens[2] : null;
                    command = new DocCommand(console, typeString, filename);
                }
                else
                {
                    return HelpCommand.MakeUnhandled(console, type);
                }
            }
            catch
            {
                string msg = string.Format("Invalid arguments for type {0}.\nEnter 'help {0}' for details.", type);
                return HelpCommand.MakeGeneric(console, msg);
            }

            return command;
        }

        
        public string GetUsage(Keyword type)
        {
 
            if (Usages.ContainsKey(type))
            {
                return Usages[type];
            }
            else
            {
                return string.Format("\n    Usage message for {0} not implemented", type);
            }
        }

        
    }
}
