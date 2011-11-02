using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.loading
{
    class EnemyLoader
    {
        private string getFilename(string name)
        {
            return string.Format("data/enemies/{0}.enemy", name);
            
        }
        public Enemy LoadByFile(string filename)
        {
            Data enemyData = new Data(filename);
            DataSection infoSection = enemyData[0];
            if (!infoSection.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ParseError("info section must come first");
            }
            EnemyInfo info = new EnemyInfo(filename, infoSection.Tokens);

            DataSection behaviorSection = enemyData[1];
            List<List<ActionBase>> actions = Loaders.Script.RawFromLines(filename, behaviorSection.Lines);
            BehaviorScript behavior = new BehaviorScript(new ScriptBase(filename, actions));
            Enemy enemy = new Enemy(info.Body.Model,
                                    info.Body.Texture,
                                    info.Body.Scale.Value,
                                    info.Body.Radius.Value,
                                    info.Body.Rotation.Value,
                                    info.Health.Value,
                                    info.Defence.Value,
                                    info.arsenal.Copy(),
                                    behavior,
                                    info.deathScript,
                                    info.deathSound,
                                    info.itembatch,
                                    info.gibbatch,
                                    info.alignment.Value);
            //fixme cache!
            return enemy;

            


        }
        /*public Enemy LoadByName(string name)
        {
            string filename = getFilename(name);
            return LoadByName(filename);
        }*/
    }
}
