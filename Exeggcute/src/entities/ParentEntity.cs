using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A parent entity contains an arsenal.
    /// </summary>
    class ParentEntity : ScriptedEntity
    {
        protected NewArsenal arsenal;
        protected HashList<Gib> gibList;
        protected HashList<Shot> shotList;

        public ParentEntity(Model model,
                            BehaviorScript behavior,
                            NewArsenal arsenal,
                            HashList<Shot> shotListHandle,
                            HashList<Gib> gibListHandle)
            : base(model, behavior)
        {
            this.gibList = gibListHandle;
            this.shotList = shotListHandle;
            Console.WriteLine(shotListHandle.Name);
            this.arsenal = arsenal;
        }

        public override void Process(ShootAction shoot)
        {
            arsenal.Process(shoot);
            actionPtr += 1;
        }

        public override void Update()
        {
 	        base.Update();

            arsenal.Update(Position, Angle);
        }

        public void ParentEntityBaseUpdate()
        {
            base.Update();
        }
    }
}
