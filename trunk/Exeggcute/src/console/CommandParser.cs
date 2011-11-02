using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Exeggcute.src.scripting;

namespace Exeggcute.src.console
{
    class CommandParser
    {
        public static Dictionary<Keyword, string> Usages { get; protected set; }
        private static Dictionary<Keyword, string[]> aliasHelper;
        public static Dictionary<string, Keyword> Aliases;
        static CommandParser()
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
                { Keyword.Doc, DocCommand.Usage },
                { Keyword.WhatIs, WhatIsCommand.Usage },
                { Keyword.SetParam, SetParamCommand.Usage },
                { Keyword.Track, TrackCommand.Usage },
                { Keyword.Clear, ClearCommand.Usage },
                { Keyword.SetGlobal, SetGlobalCommand.Usage },
            };

            aliasHelper = new Dictionary<Keyword, string[]>
            {
                { Keyword.Help, new string[] { "h" }  },
                { Keyword.Exit, new string[] { "quit" } },
                { Keyword.LevelTask, new string[] { "task" } },
                { Keyword.SetGlobal, new string[] { "setglob", "glob", "global"} },
                { Keyword.SetParam, new string[] { "set" } }
            };
            Aliases = new Dictionary<string, Keyword>();
            foreach (var pair in aliasHelper)
            {
                Keyword word = pair.Key;
                foreach (string alias in pair.Value)
                {
                    Aliases[alias] = word;
                }
            }
        }

        public Keyword getKeyword(string name)
        {
            if (Aliases.ContainsKey(name))
            {
                return Aliases[name];
            }
            else
            {
                int dummy;
                bool isInt = int.TryParse(name, out dummy);
                if (isInt) throw new ParseError("invalid");
                return Util.ParseEnum<Keyword>(name);
            }
        }

        public ConsoleCommand Parse(DevConsole console, string input)
        {
            string[] tokens = input.TrimStart(' ').Split(' ');
            string commandTypeString = tokens[0];
            Keyword type;
            try
            {
                type = getKeyword(commandTypeString);
            }
            catch
            {
                Builtin cmd;
                try
                {
                    cmd = Util.ParseEnum<Builtin>(tokens[0]);
                    return parseBuiltin(console, cmd, tokens);
                }
                catch
                {
                    return HelpCommand.MakeTypeFailure(console, commandTypeString);
                }
            }

            try
            {
                if (type == Keyword.Go)
                {
                    return new GoCommand(console, tokens[1]);
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
                            string msg = string.Format("No known help topic named \"{0}\"", tokens[1]);
                            return HelpCommand.MakeGeneric(console, msg);
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
                    return new ListCommand(console, fileType);
                }
                else if (type == Keyword.Spawn)
                {
                    SpawnType spawnType;
                    string typeString = tokens[1];
                    string name;
                    string posString;
                    string angleString;
                    if (tokens.Length >= 3)
                    {
                        name = tokens[2];
                    }
                    else
                    {
                        name = "debug";
                    }

                    if (tokens.Length >= 4)
                    {
                        posString = tokens[3];
                    }
                    else
                    {
                        posString = "(0,0,0)";
                    }

                    if (tokens.Length >= 5)
                    {
                        angleString = tokens[4];
                    }
                    else
                    {
                        angleString = "0";
                    }
                    
                    
                    
                    try
                    {
                        spawnType = Util.ParseEnum<SpawnType>(typeString);
                    }
                    catch
                    {
                        string msg = string.Format("No SpawnType with name \"{0}\"\n{1}", typeString, SpawnCommand.Usage);
                        return HelpCommand.MakeGeneric(console, msg);
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
                    return new SpawnCommand(console, spawnType, name, pos, angle);
                }
                else if (type == Keyword.LoadSet)
                {
                    string name = tokens[1];
                    return new LoadSetCommand(console, name);
                }
                else if (type == Keyword.Package)
                {
                    string name = tokens[1];
                    return new PackageCommand(console, name);
                }
                else if (type == Keyword.Reset)
                {
                    return new ResetCommand(console);
                }
                else if (type == Keyword.Exit)
                {
                    return new ExitCommand(console);
                }
                else if (type == Keyword.LevelTask)
                {
                    List<string> toks = tokens.ToList();
                    toks.RemoveAt(0);
                    string taskString = Util.Join(toks, ' ');
                    return new LevelTaskCommand(console, taskString);
                }
                else if (type == Keyword.Doc)
                {
                    string typeString = tokens[1];

                    string filename = tokens.Length >= 3 ? tokens[2] : null;
                    return new DocCommand(console, typeString, filename);
                }
                else if (type == Keyword.WhatIs)
                {
                    string typeName = tokens[1];
                    return new WhatIsCommand(console, typeName);
                }
                else if (type == Keyword.SetParam)
                {
                    int id = int.Parse(tokens[1]);
                    string paramName = tokens[2];
                    int paramIndex = Entity3D.NameToIndex(paramName);
                    if (paramIndex == -1) throw new ParseError("{0} is not a settable parameter", paramName);
                    FloatValue value = FloatValue.Parse(tokens[3]);
                    return new SetParamCommand(console, id, paramIndex, value);

                }
                else if (type == Keyword.Track)
                {
                    string[] specTokens = Util.SplitQuoteTokens(input);

                    int id = int.Parse(specTokens[1]);
                    string format = specTokens[2];
                    List<int> indices = new List<int>();
                    int i;
                    for (i = 3; i < specTokens.Length; i += 1)
                    {
                        int index = Entity3D.NameToIndex(specTokens[i]);
                        if (index == -1) break;
                        indices.Add(index);
                    }
                    if (indices.Count == 0)
                    {
                        throw new ParseError("Must specify at least one parameter to track");
                    }
                    int frequency;
                    if (i < specTokens.Length)
                    {
                        frequency = int.Parse(specTokens[i++]);
                    }
                    else
                    {
                        frequency = 0;
                    }

                    string trackType = specTokens[i++];

                    return new TrackCommand(console, id, format, indices, frequency, trackType);


                }
                else if (type == Keyword.Clear)
                {
                    string thing = tokens.Length == 1? null : tokens[1];
                    return new ClearCommand(console, thing);
                }
                else if (type == Keyword.SetGlobal)
                {
                    string param = tokens[1];
                    string value = tokens[2];
                    return new SetGlobalCommand(console, param, value);
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
        }

        protected ConsoleCommand parseBuiltin(DevConsole console, Builtin cmd, string[] tokens)
        {
            return new BuiltinCommand(console, cmd);
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
