using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    class SongBank
    {
        static private Bank<Song> bank =
            new Bank<Song>("ExeggcuteContent/songs", "xnb");

        public static Song Get(string name)
        {
            return bank[name];
        }

        public static List<string> GetLoaded()
        {
            return bank.GetAllLoaded();
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, string name)
        {
            bank.Load(content, name);
        }
    }
}
