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

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A parent entity contains an arsenal.
    /// </summary>
    class ParentEntity : ScriptedEntity
    {
        protected Arsenal arsenal;
        protected HashList<Gib> gibList;
        protected HashList<Shot> shotList;

        public ParentEntity(Model model,
                            Texture2D texture, 
                            BehaviorScript behavior,
                            Arsenal arsenal,
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, behavior)
        {
            this.gibList = gibListHandle;
            this.shotList = shotListHandle;
            this.arsenal = arsenal;
        }

        /// <summary>
        /// For use with a boss only.
        /// </summary>
        public ParentEntity(Model model,
                            Texture2D texture, 
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, texture, (BehaviorScript)null)//HACK LOLMAO
        {
            this.gibList = gibListHandle;
            this.shotList = shotListHandle;
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
