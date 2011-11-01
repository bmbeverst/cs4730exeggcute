using Exeggcute.src.assets;
using Exeggcute.src.loading.specs;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading
{
    class OptionLoader : LoadedInfo
    {
        
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
                        var rawActions = Loaders.Script.GetRaw(filename, section.Lines);
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
}
