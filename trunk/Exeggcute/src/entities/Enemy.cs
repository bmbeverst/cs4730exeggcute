using System;
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
using Exeggcute.src.scripting.arsenal;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.sound;

namespace Exeggcute.src.entities
{
    class Enemy : ParentEntity
    {

        /// <summary>
        /// If the Enemy's health is zero, then they should process their
        /// OnDeath script until done, then set IsDestroyed to true.
        /// </summary>
        public bool IsDying { get; protected set; }

        protected HashList<Item> itemListHandle;

        protected BehaviorScript deathScript;

        protected ItemBatch heldItems;

        public Enemy(Model model,
                     Texture2D texture,
                     float scale,
                     int health,
                     int defence,
                     Arsenal arsenal,
                     BehaviorScript behavior, 
                     BehaviorScript deathScript,
                     RepeatedSound deathSound,
                     ItemBatch items,
                     GibBatch gibBatch,
                     HashList<Shot> shotHandle,
                     HashList<Gib> gibHandle,
                     HashList<Item> itemHandle)
            : base(model, texture, scale, behavior, deathSound, arsenal, gibBatch, shotHandle, gibHandle)
        {
            this.Health = health;
            this.Defence = defence;
            this.arsenal = arsenal;
            this.heldItems = items;
            this.deathScript = deathScript;
            this.itemListHandle = itemHandle;
        }
                     

        /// <summary>
        /// TODO FIXME (8:10:51 AM) ZRP: i can just have 
        ///setParam health 100
        ///setParam defence 10
        /// </summary>
        /*public Enemy(RosterEntry entry, 
                     ScriptInstance deathScript, 
                     ItemBatch heldItems, 
                     HashList<Shot> enemyShots, 
                     HashList<Gib> gibList, 
                     HashList<Item> itemList)
            : base(entry.Surface, entry.Texture, entry.Behavior, entry.GetArsenal(World.EnemyShots), enemyShots, gibList)
        {
            Health = 100;
            this.rosterParams = entry;
            this.shotListHandle = shotList;
            this.gibListHandle = gibList;
            this.heldItems = heldItems;
            this.deathScript = GetDeathScript();
            this.itemListHandle = World.ItemList;
        }*/

        public Enemy Clone(Float3 pos, FloatValue angle)
        {
            Enemy cloned = new Enemy(Surface, 
                                     Texture, 
                                     Scale, 
                                     Health, 
                                     Defence, 
                                     arsenal,
                                     (BehaviorScript)script,
                                     deathScript,
                                     deathSound,
                                     heldItems.Clone(),
                                     gibBatch,
                                     shotListHandle, 
                                     gibListHandle, 
                                     itemListHandle);
            cloned.Position = pos.Vector3;
            cloned.Angle = angle.Value;//fixme this does nothing?
            return cloned;
        }

        public static BehaviorScript GetDeathScript()
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


        public override void Kill()
        {
            IsTrash = true;
            Vector3 deathpos = new Vector3(X, Y, 0);
            Model gibModel = ModelBank.Get("junk0");
            Texture2D texture = TextureBank.Get("junk");
            foreach (Gib gib in gibBatch.gibs)
            {
                //FIXME double copying
                Gib newGib = new Gib(gib.Surface, gib.Texture, gib.Scale, Position2D, Speed, Angle);
                gibListHandle.Add(newGib);
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
