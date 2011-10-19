using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.entities
{
    class Enemy : ParentEntity
    {

        /// <summary>
        /// If the Enemy's health is zero, then they should process their
        /// OnDeath script until done, then set IsDestroyed to true.
        /// </summary>
        public bool IsDying { get; protected set; }

        protected ModelName modelName;
        protected ScriptName scriptName;
        protected ArsenalName arsenalName;

        protected HashList<Shot> shotListHandle;
        protected HashList<Gib> gibListHandle;
        protected ActionList deathActions;
        protected static ScriptName deathScriptName = ScriptName.death0;

        protected RosterEntry rosterParams;
        /// <summary>
        /// TODO FIXME (8:10:51 AM) ZRP: i can just have 
        ///setParam health 100
        ///setParam defence 10
        /// </summary>
        public Enemy(RosterEntry entry, HashList<Shot> enemyShots, HashList<Gib> gibList)
            : base(entry.ModelName, entry.ScriptName, entry.ArsenalName, enemyShots, gibList)
        {
            Health = 100;
            this.rosterParams = entry;
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
            this.arsenalName = entry.ArsenalName;
            this.shotListHandle = shotList;
            this.gibListHandle = gibList;
            this.deathActions = ScriptBank.Get(deathScriptName);
        }

        public Enemy Clone(EntityArgs args)
        {
            Enemy cloned = new Enemy(rosterParams, shotListHandle, gibListHandle);
            cloned.Position = args.SpawnPosition;
            cloned.Angle = args.AngleHeading;
            return cloned;
        }

        public override void Update()
        {
            if (!IsDying)
            {
                base.Update();
            }
            else
            {
                ProcessPhysics();
            }

            //Console.WriteLine("{0} {1} {2}", IsDying, cmdPtr, IsShooting);
            if (Health <= 0 && !IsDying)
            {
                World.DyingList.Add(this);
                IsDying = true;
                actionList = deathActions;
                //Console.WriteLine(deathActions.Name.ToString());
                actionPtr = 0;
                arsenal.StopAll();
                //use giblist
                //TODO make enemies transparent when dying
                
            }
            else if (IsDying)
            {
                //Console.WriteLine(cmdPtr);
                if (actionPtr == actionList.Count)
                {
                    IsTrash = true;
                }
            }

        }

        int numGibs = 4;
        public void Die()
        {
            IsTrash = true;
            for (int i = 0; i < numGibs; i += 1)
            {
                gibListHandle.Add(new Gib(ModelName.XNAface, Position2D, Speed, Angle));
            }
        }

    }
}
