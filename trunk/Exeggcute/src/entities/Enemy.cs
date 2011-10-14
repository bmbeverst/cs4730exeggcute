using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class Enemy : CommandEntity
    {
        public Enemy(ModelName name, ScriptName script, List<Shot> spawnList, HashList<Shot> shotList)
            : base(name, script, spawnList, shotList)
        {

        }
    }
}
