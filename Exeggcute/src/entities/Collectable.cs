using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class Collectable : CommandEntity
    {
        public Collectable(ModelName name)
            //----------------------------------------------Collectables *never* spawn shots
            : base(name, ScriptName.item, new List<Shot>(), new List<Shot>())
        {

        }
    }
}
