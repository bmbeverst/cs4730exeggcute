using System;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            cloned.Angle = angle.Value;
            Console.WriteLine("CLONE! {0} {1}", cloned.ActionPtr, cloned.WaitCounter);
            return cloned;
        }



        public static Enemy LoadFromFile(string filepath)
        {
            return Loaders.Enemy.LoadByFile(filepath);
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
                ActionPtr = 0;
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
