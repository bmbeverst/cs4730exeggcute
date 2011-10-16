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
        protected ScriptName spawnerName;

        public Enemy(ModelName modelName, 
                     ScriptName scriptName, 
                     ArsenalName arsenalName,
                     ScriptName spawnerName,
                     HashList<Shot> enemyShots)
            : base(modelName, scriptName, arsenalName, spawnerName, enemyShots)
        {
            this.modelName = modelName;
            this.scriptName = scriptName;
            this.arsenalName = arsenalName;
            this.spawnerName = spawnerName;
            
        }

        public Enemy(RosterEntry entry, HashList<Shot> enemyShots)
            : base(entry.ModelName, entry.ScriptName, entry.ArsenalName, entry.SpawnerName, enemyShots)
        {
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
            this.arsenalName = entry.ArsenalName;
            this.spawnerName = entry.SpawnerName;
        }

        public Enemy Clone(EntityArgs args)
        {
            Enemy cloned = new Enemy(modelName, scriptName, arsenalName, spawnerName, ShotList);
            cloned.Position = args.SpawnPosition;
            cloned.Angle = args.AngleHeading;
            return cloned;
        }

    }
}
