using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.loading
{
    class EnemyLoader
    {
        public static Enemy Load(string name)
        {
            string filepath = string.Format("data/enemies/{0}.enemy", name);
            Data enemyData = new Data(filepath);
            DataSection infoSection = enemyData[0];
            if (!infoSection.Tag.Equals("info",  StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ParseError("info section must come first");
            }
            EnemyInfo info = new EnemyInfo(infoSection.Tokens);

            DataSection behaviorSection = enemyData[1];
            ScriptLoader scriptLoader = new ScriptLoader();
            List<List<ActionBase>> actions = scriptLoader.RawFromLines(behaviorSection.Lines);
            BehaviorScript behavior = new BehaviorScript(new ScriptBase(filepath, actions));
            Enemy enemy = new Enemy(info.Body.Model,
                                    info.Body.Texture,
                                    info.Body.Scale.Value,
                                    info.Health.Value,
                                    info.Defence.Value,
                                    info.arsenal,
                                    behavior,
                                    info.deathScript,
                                    info.shootSFX,
                                    info.dieSFX,
                                    info.itembatch,
                                    info.gibbatch,
                                    World.EnemyShots,
                                    World.GibList,
                                    World.ItemList);
            //fixme cache!
            return enemy;

            


        }
    }
}
