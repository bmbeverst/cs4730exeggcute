using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A parent entity contains an arsenal.
    /// </summary>
    class ParentEntity : CommandEntity
    {
        protected NewArsenal arsenal;
        protected HashList<Gib> gibList;
        protected HashList<Shot> shotList;

        public ParentEntity(ModelName modelName,
            ScriptName scriptName,
            ArsenalName arsenalName,
            HashList<Shot> shotListHandle,
            HashList<Gib> gibListHandle)
            : base(modelName, scriptName)
        {
            List<ArsenalEntry> entries = ArsenalBank.Get(arsenalName);
            this.gibList = gibListHandle;
            this.shotList = shotListHandle;
            Console.WriteLine(shotListHandle.Name);
            this.arsenal = new NewArsenal(entries, shotListHandle);
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
