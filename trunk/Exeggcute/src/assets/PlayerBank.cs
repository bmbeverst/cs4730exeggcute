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
        protected static CustomBank<Player> standardBank =
            new CustomBank<Player>("data/players/standard");
        protected static CustomBank<Player> customBank =
            new CustomBank<Player>("data/players/custom");
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

        public static void LoadAll()
        {
            foreach (string file in standardBank.AllFiles)
            {
                try
                {
                    string name = standardBank.GetName(file);
                    standardBank.Put(loader.Load(name, false), file);
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
                    standardBank.Put(loader.Load(name, true), file);
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
