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
        protected static CustomBank<Player> bank =
            new CustomBank<Player>("data/players");
        public static bool IsLoaded { get; protected set; }
        protected static PlayerLoader loader = new PlayerLoader();
        public static Player Get(string name)
        {
            return bank[name];
        }

        public bool ContainsKey(string name)
        {
            return bank.ContainsKey(name);
        }

        public static List<Player> GetAll()
        {
            return bank.GetAssets();
        }

        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                bool isCustom = Regex.IsMatch(file, "custom");
                string name = bank.GetName(file);
                if (bank.ContainsKey(name))
                {
                    throw new ExeggcuteError("Player names must be unique in the two folders");
                }
                bank.Put(loader.Load(name, isCustom), file);
            }
            IsLoaded = true;
        }
    }
}
