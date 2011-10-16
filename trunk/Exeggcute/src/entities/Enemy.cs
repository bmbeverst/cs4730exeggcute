using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.entities
{
    class Enemy : CommandEntity
    {

        /// <summary>
        /// If the Enemy's health is zero, then they should process their
        /// OnDeath script until done, then set IsDestroyed to true.
        /// </summary>
        public bool IsDying { get; protected set; }

        /// <summary>
        /// Signifies to CollisionManager to remove this entity from the worlf
        /// immediately.
        /// </summary>
        public bool IsDestroyed { get; protected set; }

        protected ModelName modelName;
        protected ScriptName scriptName;
        protected ArsenalName arsenalName;
        protected ScriptName spawnerName;
        protected ScriptName deathScriptName;

        protected HashList<Gib> gibList;
        protected ActionList deathActions;

        public Enemy(ModelName modelName, 
                     ScriptName scriptName, 
                     ArsenalName arsenalName,
                     ScriptName spawnerName,
                     ScriptName deathScriptName,
                     HashList<Shot> enemyShots,
            HashList<Gib> gibList)
            : base(modelName, scriptName, arsenalName, spawnerName, enemyShots)
        {
            this.modelName = modelName;
            this.scriptName = scriptName;
            this.arsenalName = arsenalName;
            this.spawnerName = spawnerName;
            this.deathScriptName = deathScriptName;
            this.deathActions = ScriptBank.Get(deathScriptName);
            this.gibList = gibList;
            
        }

        public Enemy(RosterEntry entry, HashList<Shot> enemyShots, HashList<Gib> gibList)
            : base(entry.ModelName, entry.ScriptName, entry.ArsenalName, entry.SpawnerName, enemyShots)
        {
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
            this.arsenalName = entry.ArsenalName;
            this.spawnerName = entry.SpawnerName;
            this.deathActions = ScriptBank.Get(deathScriptName);
            this.gibList = gibList;
        }

        public Enemy Clone(EntityArgs args)
        {
            Enemy cloned = new Enemy(modelName, scriptName, arsenalName, spawnerName, deathScriptName, ShotList, gibList);
            cloned.Position = args.SpawnPosition;
            cloned.Angle = args.AngleHeading;
            return cloned;
        }

        public override void Update()
        {
            base.Update();
            if (Health <= 0 && !IsDying)
            {
                IsDying = true;
                actionList = deathActions;
                cmdPtr = 0;
            }
            else if (IsDying)
            {
                if (cmdPtr == actionList.Count)
                {
                    IsDestroyed = true;
                }
            }
        }

    }
}
