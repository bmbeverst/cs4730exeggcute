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

        protected ModelName modelName;
        protected ScriptName scriptName;
        protected ArsenalName arsenalName;
        protected ScriptName spawnerName;
        protected ScriptName deathScriptName;

        protected HashList<Gib> gibListHandle;
        protected List<Gib> gibs;
        protected ActionList deathActions;

        /// <summary>
        /// (8:10:51 AM) ZRP: i can just have 
        ///setParam health 100
        ///setParam defence 10
        /// </summary>
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
            this.gibListHandle = gibList;
            
        }

        public Enemy(RosterEntry entry, HashList<Shot> enemyShots, HashList<Gib> gibList)
            : base(entry.ModelName, entry.ScriptName, entry.ArsenalName, entry.SpawnerName, enemyShots)
        {
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
            this.arsenalName = entry.ArsenalName;
            this.spawnerName = entry.SpawnerName;
            this.deathScriptName = entry.DeathScriptName;
            this.deathActions = ScriptBank.Get(deathScriptName);
            this.gibListHandle = gibList;
        }

        public Enemy Clone(EntityArgs args)
        {
            Enemy cloned = new Enemy(modelName, scriptName, arsenalName, spawnerName, deathScriptName, shotListHandle, gibListHandle);
            cloned.Position = args.SpawnPosition;
            cloned.Angle = args.AngleHeading;
            return cloned;
        }

        public override void Update()
        {
            base.Update();
            //Console.WriteLine("{0} {1} {2}", IsDying, cmdPtr, IsShooting);
            if (Health <= 0 && !IsDying)
            {
                IsDying = true;
                actionList = deathActions;
                //Console.WriteLine(deathActions.Name.ToString());
                cmdPtr = 0;
                IsShooting = false;
                //use giblist
                //TODO make enemies transparent when dying
                gibListHandle.Add(new Gib(ModelName.playerScene, Position2D, Speed, Angle));
                
            }
            else if (IsDying)
            {
                //Console.WriteLine(cmdPtr);
                if (cmdPtr == actionList.Count)
                {
                    IsTrash = true;
                    Console.WriteLine("DEADED {0}", IsTrash);
                }
            }
        }

    }
}
