using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.roster;

namespace Exeggcute.src.entities
{
    class Enemy : CommandEntity
    {

        public bool IsDestroyed
        {
            get { return Health <= 0; }
        }

        protected ModelName modelName;
        protected ScriptName scriptName;
        protected ArsenalName arsenalName;

        public Enemy(ModelName modelName, ScriptName scriptName, ArsenalName arsenalName, HashList<Shot> enemyShots)
            : base(modelName, scriptName, arsenalName, enemyShots)
        {
            this.modelName = modelName;
            this.scriptName = scriptName;
            this.arsenalName = arsenalName;
            
        }

        public Enemy(RosterEntry entry, HashList<Shot> enemyShots)
            : base(entry.ModelName, entry.ScriptName, entry.ArsenalName, enemyShots)
        {
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
            this.arsenalName = entry.ArsenalName;
        }

        public Enemy Clone(EntityParams args)
        {
            return new Enemy(modelName, scriptName, arsenalName, ShotList);
        }

    }
}
