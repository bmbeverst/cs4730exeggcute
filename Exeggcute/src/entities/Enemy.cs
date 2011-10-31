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
using Exeggcute.src.loading;

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
                     float radius,
                     Vector3 rotation,
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
            : base(model, texture, scale, radius, rotation, behavior, deathSound, arsenal, gibBatch, shotHandle, gibHandle)
        {
            this.Health = health;
            this.Defence = defence;
            this.arsenal = arsenal;
            this.heldItems = items;
            this.deathScript = deathScript;
            this.itemListHandle = itemHandle;
        }
                     

        public Enemy Clone(Float3 pos, FloatValue angle)
        {
            Enemy cloned = new Enemy(Surface, 
                                     Texture, 
                                     Scale, 
                                     Radius,
                                     ModelRotation,
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

        protected static EnemyLoader loader = new EnemyLoader();

        public static Enemy LoadFromFile(string filepath)
        {
            return loader.LoadByFile(filepath);
        }

        public static BehaviorScript GetDeathScript()
        {
            return Assets.GetBehavior("death0"); 
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

            if (Health <= 0 && !IsDying)
            {
                World.DyingList.Add(this);
                IsDying = true;
                script = deathScript;
                actionPtr = 0;
                arsenal.StopAll();
                //use giblist
                //TODO make enemies transparent when dying
                
            }


        }


        public override void Kill()
        {
            IsTrash = true;
            Vector3 deathpos = new Vector3(X, Y, 0);
            Model gibModel = Assets.Model["junk0"];
            Texture2D texture = Assets.Texture["junk"];
            foreach (Gib gib in gibBatch.gibs)
            {
                //FIXME double copying
                Gib newGib = new Gib(gib.Surface, gib.Texture, gib.Scale,gib.Radius, gib.ModelRotation, Position2D, Speed, Angle);
                gibListHandle.Add(newGib);
            }
            heldItems.Release(itemListHandle, deathpos);
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
        }

    }
}
