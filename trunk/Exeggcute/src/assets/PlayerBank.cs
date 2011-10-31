using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.loading;
using System.Text.RegularExpressions;

namespace Exeggcute.src.assets
{
    class PlayerBank
    {
        protected static DataBank<Player> standardBank =
            new DataBank<Player>("players/standard", "player");
        protected static DataBank<Player> customBank =
            new DataBank<Player>("players/custom", "player");
        public static bool IsLoaded { get; protected set; }
        protected static PlayerLoader loader = new PlayerLoader();
        public static Player Get(string name, bool isCustom)
        {
            if (isCustom)
            {
                return customBank[name];
            }
            else
            {
                return standardBank[name];
            }
        }

        public bool ContainsKey(string name, bool isCustom)
        {
            if (isCustom)
            {
                return customBank.ContainsKey(name);
            }
            else
            {
                return standardBank.ContainsKey(name);
            }
        }

        public static List<Player> GetAll(bool isCustom)
        {
            if (isCustom)
            {
                return customBank.GetAssets();
            }
            else
            {
                return standardBank.GetAssets();
            }
        }

        public static bool Exists(string name)
        {
            return standardBank.ContainsKey(name) ||
                   customBank.ContainsKey(name);
        }

        /// <summary>
        /// Returns whether or not the given name refers to a custom player.
        /// Assumes that the player exists in one of the banks which must
        /// be checked before calling using this Exists function.
        /// </summary>
        public static bool IsCustom(string name)
        {
            if (standardBank.ContainsKey(name))
            {
                return false;
            }
            else if (customBank.ContainsKey(name))
            {
                return true;
            }
            else
            {
                throw new ExeggcuteError("Check to see if exists first!");
            }
        }

        public static void LoadAll()
        {
            foreach (string file in standardBank.AllFiles)
            {
                try
                {
                    string name = standardBank.GetName(file);
                    standardBank.PutByFile(loader.Load(name, false), file);
                }
                catch (Exception e)
                {
                    AssetManager.LogFailure("Failed to load player {0}:\n{1}", file, e.Message);
                }
            }

            foreach (string file in customBank.AllFiles)
            {
                try
                {
                    string name = customBank.GetName(file);
                    customBank.PutByFile(loader.Load(name, true), file);
                }
                catch (Exception e)
                {
                    AssetManager.LogFailure("Failed to load player {0}:\n{1}", file, e.Message);
                }
            }
            IsLoaded = true;
        }
    }
}
