using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.sound;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A parent entity contains an arsenal.
    /// </summary>
    class ParentEntity : ScriptedEntity
    {
        protected Arsenal arsenal;
        protected HashList<Gib> gibListHandle;
        protected HashList<Shot> shotListHandle;

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
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, scale, radius, rotation, behavior)
        {
            this.deathSound = deathSound;
            this.gibListHandle = gibListHandle;
            this.shotListHandle = shotListHandle;
            this.arsenal = arsenal;
            this.gibBatch = gibBatch;
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
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, scale, radius, rotation, (BehaviorScript)null)//HACK LOLMAO
        {
            this.gibListHandle = gibListHandle;
            this.shotListHandle = shotListHandle;
            this.gibBatch = gibBatch;
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
            actionPtr += 1;
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


        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            arsenal.Draw(graphics, view, projection, Position);
        }

        public virtual void Kill()
        {
            deathSound.Play();
        }
    }
}
