using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.sound;

namespace Exeggcute.src.loading.specs
{
#pragma warning disable 0649
    class OptionInfo : Loadable
    {
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
            return Loaders.Option.Load(filename);
        }

        public static OptionInfo Parse(String name)
        {
            string filename = getFilename(name);
            if (!Assets.Option.ContainsKey(name))
            {
                Util.Die("why is this necessary?");
                return Loaders.Option.Load(filename);
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
