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
        private static string getFilename(string name, string folder)
        {
            return string.Format("data/players/{0}/{1}.player", folder, name);
        }
        public Player Load(string name, bool isCustom)
        {

            string folder = isCustom ? "custom" : "standard";
            string filename = getFilename(name, folder);
            Data data = new Data(filename);


            DataSection infoSection = data[0];
            if (!infoSection.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase ))
            {
                throw new ParseError("info section must come first");
            }
            PlayerInfo info = new PlayerInfo(filename, infoSection.Tokens);


            //// load the player's weapon//////////////////////////////
            List<Arsenal> weapons = new List<Arsenal>();
            List<int> thresholds = new List<int>();
            for (int k = 1; k < data.Count; k += 1)
            {
                //FIXME really?...
                DataSection currentSection = data[k];
                List<string> tokens =  currentSection.TagValue.Split(',').ToList();
                string thresh = tokens[0];
                
                tokens.RemoveAt(0);
                string recombined = Util.Join(tokens, ',');

                weapons.Add(Arsenal.Parse(recombined));
                thresholds.Add(int.Parse(thresh));

            }
            /////////////////////////////////////////////////////
           
            return new Player(data.RawText,
                              name,
                              isCustom,
                              info.body.Model,
                              info.body.Texture,
                              info.body.Scale.Value, 
                              info.body.Radius.Value,
                              info.body.Rotation.Value,
                              info.deathScript,
                              info.special,  
                              info.gibBatch,
                              info.deathSound,
                              weapons, 
                              thresholds,
                              info.lives.Value,
                              info.bombs.Value,
                              info.moveSpeed.Value,
                              info.focusSpeed.Value,
                              
                              info.hitRadius.Value,
                              info.lightLevel.Value,
                              World.PlayerShots, 
                              World.GibList);
        }
    }

    
}
