using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.loading;
using Exeggcute.src.sound;
using System.Text.RegularExpressions;

namespace Exeggcute.src.scripting.arsenal
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
            loadFromFile(Filename);
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
            loadFromTokens(tokenList);
        }

        public OptionInfo()
        {

        }

        public static OptionInfo LoadFromFile(string filename)
        {
            return new OptionInfo(filename);
        }

        public static OptionInfo Parse(String name)
        {
            string filename = getFilename(name);
            return new OptionInfo(filename);
        }

        private static string getFilename(string name)
        {
            return string.Format("data/options/{0}.option", name);
            
        }
    }
}
