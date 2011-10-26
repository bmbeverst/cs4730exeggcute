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

        protected SoundEffect shootSFX;
        protected SoundEffect dieSFX;

        public ParentEntity(Model model,
                            Texture2D texture, 
                            float scale,
                            BehaviorScript behavior,
                            SoundEffect shootSFX,
                            SoundEffect dieSFX,
                            Arsenal arsenal,
                            GibBatch gibBatch,
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, scale, behavior)
        {
            this.shootSFX = shootSFX;
            this.dieSFX = dieSFX;
            this.gibListHandle = gibListHandle;
            this.shotListHandle = shotListHandle;
            this.arsenal = arsenal;
        }

        /// <summary>
        /// For use with a boss only.
        /// </summary>
        public ParentEntity(Model model,
                            Texture2D texture, 
                            float scale, 
                            GibBatch gibBatch,
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, scale, (BehaviorScript)null)//HACK LOLMAO
        {
            this.gibListHandle = gibListHandle;
            this.shotListHandle = shotListHandle;
            this.gibBatch = gibBatch;
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
    }
}
