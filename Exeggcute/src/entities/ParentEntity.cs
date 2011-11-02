using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A parent entity contains an arsenal.
    /// </summary>
    class ParentEntity : ScriptedEntity
    {
        protected Arsenal arsenal;

        protected GibBatch gibBatch;

        protected RepeatedSound deathSound;

        public ParentEntity(Model model,
                            Texture2D texture, 
                            float scale,
                            float radius,
                            Vector3 rotation,
                            BehaviorScript behavior,
                            RepeatedSound deathSound,
                            Arsenal arsenal,
                            GibBatch gibBatch,
                            Alignment alignment)
            : base(model, texture, scale, radius, rotation, behavior)
        {
            this.deathSound = deathSound;
            this.arsenal = arsenal;
            this.gibBatch = gibBatch;
            this.Alignment = alignment;
            
        }

        /// <summary>
        /// For use with a boss only.
        /// </summary>
        public ParentEntity(Model model,
                            Texture2D texture, 
                            float scale,
                            float radius,
                            Vector3 rotation,
                            GibBatch gibBatch,
                            Alignment alignment)
            : base(model, texture, scale, radius, rotation, (BehaviorScript)null)//HACK LOLMAO
        {
            this.gibBatch = gibBatch;
            this.Alignment = alignment;
        }

        //fixme code clone in spawner
        protected void chooseAlignment(Alignment alignment)
        {
            if (alignment == src.Alignment.None)
            {
                throw new ExeggcuteError("this is an invalid alignment");
            }
            arsenal.CheckAlignment(alignment);
        }


        /// <summary>
        /// Used to allow subclasses to implement ILoadable
        /// </summary>
        protected ParentEntity()
        {

        }

        public override void Process(ShootAction shoot)
        {
            arsenal.Fire(shoot);
            ActionPtr += 1;
        }

        public override void Update()
        {
 	        base.Update();
            arsenal.Update(Position, Angle);
        }

        public void UpdateMovers()
        {
            arsenal.UpdateMovers(Position, Angle);


            base.Update();
        }


        public override void Draw3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw3D(graphics, view, projection);
            arsenal.Draw3D(graphics, view, projection);
        }

        public virtual void Kill()
        {
            deathSound.Play();
        }
    }
}
