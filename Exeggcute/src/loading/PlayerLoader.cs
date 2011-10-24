using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;
using System.IO;
using Exeggcute.src.loading.specs;

namespace Exeggcute.src.loading
{
    class PlayerLoader : Loader
    {
        PlayerWeaponLoader weaponLoader = new PlayerWeaponLoader();
        public Player Load(string name, bool isCustom)
        {
            PlayerInfo info = null;
            List<int> thresholds = null;
            List<Arsenal> arsenals = null;
            string folder = isCustom ? "custom" : "standard";
            string filepath = string.Format("data/players/{0}/{1}.player", folder, name);
            Data data = new Data(filepath);
            for (int k = 0; k < data.Count; k += 1)
            {
                List<string> lines = data[k].Lines;
                currentField = data[k].Tag;
                if (matches("info"))
                {
                    info = new PlayerInfo(lines);
                }
                else if (matches("weapon"))
                {
                    thresholds = new List<int>();
                    arsenals = new List<Arsenal>();
                    weaponLoader.Load(lines, thresholds, arsenals);
                }
                
            }
            if (info == null || thresholds == null || arsenals == null)
            {
                throw new ParseError("Not all fields were initialized");
            }

            
            return new Player(data.RawText,
                              name,
                              isCustom,
                              info.Surface, 
                              info.Texture,
                              info.DeathScript, 
                              info.Bomb,  
                              arsenals, 
                              thresholds,
                              info.NumLives.Value,
                              info.NumBombs.Value,
                              info.MoveSpeed.Value,
                              info.FocusSpeed.Value,
                              info.ModelScale.Value, 
                              info.HitRadius.Value,
                              World.PlayerShots, 
                              World.GibList);
        }
    }

    
}
