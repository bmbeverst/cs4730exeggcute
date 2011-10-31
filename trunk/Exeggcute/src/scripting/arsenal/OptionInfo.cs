using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.loading;
using Exeggcute.src.sound;
using System.Text.RegularExpressions;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.scripting.arsenal
{
#pragma warning disable 0649


    class OptionLoader : LoadedInfo
    {
        protected static ScriptLoader scriptLoader = new ScriptLoader();
        public OptionInfo Load(string filename)
        {
            Data data = new Data(filename);
            OptionInfo info;
            if (data.Count == 1)
            {
                //FIXME double loading
                return new OptionInfo(filename);
            }
            else
            {
                if (!Util.StrEq(data[0].Tag, "Info"))
                {
                    throw new ParseError("Info tag must come first in option");
                }
                info = new OptionInfo(filename, data[0].Tokens);
                for (int i = 1; i < data.Count; i += 1)
                {
                    DataSection section = data[i];
                   currentField = section.Tag;
                    if (Assets.ScriptExists(section.TagValue))
                    {
                        if (section.Count > 0)
                        {
                            Util.Warn("You specified a script name, but also specified an in-line script in {0}", filename);
                        }
                        string name = section.TagValue;
                        if (matches("behavior"))
                        {
                            info.Behavior = Assets.GetBehavior(name);
                        }
                        else if (matches("trajectory"))
                        {
                            info.Trajectory = Assets.GetTrajectory(name);
                        }
                        else if (matches("spawn"))
                        {
                            info.Spawn = Assets.GetSpawn(name);
                        }
                        else
                        {
                            //FIXME make helpful
                            throw new ParseError("Don't know what to do with {0}", currentField);
                        }
                    }
                    else
                    {
                        
                        string mangledName = Util.MangleName(filename, currentField);
                        var rawActions = scriptLoader.GetRaw(filename, section.Lines);
                        ScriptBase script = new ScriptBase(mangledName, rawActions);
                        
                        if (matches("behavior"))
                        {
                            Assets.Behavior.Insert(mangledName, script);
                            info.Behavior = new BehaviorScript(script);
                        }
                        else if (matches("trajectory"))
                        {
                            Assets.Trajectory.Insert(mangledName, script);
                            info.Trajectory = new TrajectoryScript(script);
                        }
                        else if (matches("spawn"))
                        {
                            Assets.Spawn.Insert(mangledName, script);
                            info.Spawn = new SpawnScript(script);
                        }
                        else
                        {
                            throw new ParseError("no script type with name {0}", currentField);
                        }
                    }

                }
            }
            AssertInitialized(this);
            return info;

        }
    }
    class OptionInfo : Loadable
    {
        protected static OptionLoader loader = new OptionLoader();
        public BodyInfo Body;
        public int Damage;
        public RepeatedSound ShotSound;
        public BehaviorScript Behavior;
        public TrajectoryScript Trajectory;
        public SpawnScript Spawn;


        public OptionInfo(string unknown)
            : base(getFileName(unknown))
        {
            loadFromFile(Filename, true);
        }

        public OptionInfo(string unknown, bool verify)
            : base(getFileName(unknown))
        {
            loadFromFile(Filename, verify);
        }

        protected static string getFileName(string unknown)
        {
            bool isFile = Regex.IsMatch(unknown, "/");
            if (isFile)
            {
                return unknown;
            }
            else
            {
                string filename = string.Format("data/options/{0}.option", unknown);
                return filename;
            }
        }

        public OptionInfo(string filename, List<string[]> tokenList)
            : base(filename)
        {
            loadFromTokens(tokenList, false);
        }

        public static OptionInfo LoadFromFile(string filename)
        {
            return loader.Load(filename);
        }

        public static OptionInfo Parse(String name)
        {
            string filename = getFilename(name);
            if (!Assets.Option.ContainsKey(name))
            {
                return loader.Load(filename);
            }
            else
            {
                return Assets.Option[name];
            }
        }

        private static string getFilename(string name)
        {
            return string.Format("data/options/{0}.option", name);
            
        }
    }
}
