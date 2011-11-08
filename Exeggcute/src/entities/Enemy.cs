using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        protected BehaviorScript deathScript;

        protected ItemBatch heldItems;

        public int BaseHealth { get; protected set; }

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
                     Alignment alignment)
            : base(model, texture, scale, radius, rotation, behavior, deathSound, arsenal, gibBatch, alignment)
        {
            this.Health = health;
            this.BaseHealth = health;
            this.Defence = defence;
            this.heldItems = items;
            this.deathScript = deathScript;
            chooseAlignment(Alignment);
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
                                     arsenal.Copy(),
                                     (BehaviorScript)script,
                                     deathScript,
                                     deathSound,
                                     heldItems.Clone(),
                                     gibBatch,
                                     Alignment);
            cloned.Position = pos.Vector3;
            cloned.Angle = angle.Value;
            Console.WriteLine(cloned.Position);
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
                Worlds.World.AddDying(this);
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
                Gib newGib = new Gib(gib.Surface, gib.Texture, gib.Scale, gib.Radius, gib.ModelRotation, Position2D, Speed, Angle);
                Worlds.World.AddGib(newGib);
            }
            Worlds.World.ReleaseItems(heldItems, deathpos);
        }

        public override void Draw3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw3D(graphics, view, projection);
            arsenal.Draw3D(graphics, view, projection);
        }

    }
}
