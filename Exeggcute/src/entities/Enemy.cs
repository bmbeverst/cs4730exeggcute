﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;
using Exeggcute.src.entities.items;

namespace Exeggcute.src.entities
{
    class Enemy : ParentEntity
    {

        /// <summary>
        /// If the Enemy's health is zero, then they should process their
        /// OnDeath script until done, then set IsDestroyed to true.
        /// </summary>
        public bool IsDying { get; protected set; }

        protected HashList<Shot> shotListHandle;
        protected HashList<Gib> gibListHandle;
        protected HashList<Item> itemListHandle;
        protected Script deathScript;

        protected RosterEntry rosterParams;

        protected ItemBatch heldItems;
        /// <summary>
        /// TODO FIXME (8:10:51 AM) ZRP: i can just have 
        ///setParam health 100
        ///setParam defence 10
        /// </summary>
        public Enemy(RosterEntry entry, 
                     Script deathScript, 
                     ItemBatch heldItems, 
                     HashList<Shot> enemyShots, 
                     HashList<Gib> gibList, 
                     HashList<Item> itemList)
            : base(entry.Surface, entry.Behavior, entry.GetArsenal(World.EnemyShots), enemyShots, gibList)
        {
            Health = 100;
            this.rosterParams = entry;
            this.shotListHandle = shotList;
            this.gibListHandle = gibList;
            this.heldItems = heldItems;
            this.deathScript = GetDeathScript();
            this.itemListHandle = World.ItemList;
        }

        public Enemy Clone(EntityArgs args)
        {
            Enemy cloned = new Enemy(rosterParams, deathScript, heldItems.Clone(), shotListHandle, gibListHandle, itemListHandle);
            cloned.Position = args.SpawnPosition.Vector;
            cloned.Angle = args.AngleHeading.Value;
            return cloned;
        }

        public static Script GetDeathScript()
        {
            return ScriptBank.GetBehavior("death0"); 
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
                script = deathScript;
                //Console.WriteLine(deathActions.Name.ToString());
                actionPtr = 0;
                arsenal.StopAll();
                //use giblist
                //TODO make enemies transparent when dying
                
            }
            /*else if (IsDying)
            {
                Util.Die("is dying");
                //Console.WriteLine(cmdPtr);
                if (actionPtr == script.Count)
                {
                    Vector3 deathpos = new Vector3(X, Y, 0);
                    Util.Die("released");
                    heldItems.Release(itemListHandle, deathpos);
                    IsTrash = true;
                }
            }*/

        }

        int numGibs = 4;
        public void Kill()
        {
            IsTrash = true;
            Vector3 deathpos = new Vector3(X, Y, 0);
            Model gibModel = ModelBank.Get("junk0");
            for (int i = 0; i < numGibs; i += 1)
            {
                gibListHandle.Add(new Gib(gibModel, Position2D, Speed, Angle));
            }
            heldItems.Release(itemListHandle, deathpos);
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            arsenal.Draw(graphics, view, projection, Position);
        }

    }
}
