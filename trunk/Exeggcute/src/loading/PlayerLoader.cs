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
        public Player Load(string name, bool isCustom)
        {

            string folder = isCustom ? "custom" : "standard";
            string filepath = string.Format("data/players/{0}/{1}.player", folder, name);
            Data data = new Data(filepath);


            DataSection infoSection = data[0];
            if (!infoSection.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase ))
            {
                throw new ParseError("info section must come first");
            }
            PlayerInfo info = new PlayerInfo(infoSection.Tokens);


            // the first entry is the special attack!///////////////////////
            /*List<OptionInfo> bombOptions = new List<OptionInfo>();
            DataSection bombSection = data[1];
            Data bombData = new Data(name, bombSection.RawText, '$');
            for (int i = 0; i < bombData.Count; i += 1)
            {
                DataSection currentArsenalSection = bombData[i];
                OptionInfo entry = new OptionInfo(currentArsenalSection.Tokens);
                bombOptions.Add(entry);
            }

            Arsenal special = new Arsenal(bombOptions, World.PlayerShots);*/
            /////////////////////////////////////////////////////////////


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

                /*Data arsenalData = new Data(name, currentSection.RawText, '$');
                List<OptionInfo> entries = new List<OptionInfo>();

                for (int i = 0; i < arsenalData.Count; i += 1)
                {
                    DataSection currentArsenal = arsenalData[i];
                    OptionInfo entry = new OptionInfo(currentArsenal.Tokens);
                    entries.Add(entry);
                }
                //Option
                int thresh = int.Parse(currentSection.TagValue);
                thresholds.Add(thresh);
                if (entries.Count == 0)
                {
                    throw new ParseError("Arsenal {0} had no entries", k);
                }
                Arsenal weap = new Arsenal(entries, World.PlayerShots);
                weapons.Add(weap);*/
            }
            /////////////////////////////////////////////////////
           
            return new Player(data.RawText,
                              name,
                              isCustom,
                              info.body.Model,
                              info.body.Texture,
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
                              info.body.Scale.Value, 
                              info.hitRadius.Value,
                              info.lightLevel.Value,
                              World.PlayerShots, 
                              World.GibList);
        }
    }

    
}
