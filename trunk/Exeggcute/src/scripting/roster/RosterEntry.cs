using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.scripting.roster
{
    class RosterEntry
    {
        public Model Surface { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public ItemBatch HeldItems { get; protected set; }
        private Arsenal arsenal;

        public RosterEntry(Model model,
                           BehaviorScript behavior, 
                           ItemBatch items,
                           Arsenal arsenal)
        {
            this.Surface = model;
            this.Behavior = behavior;
            this.HeldItems = items;
            this.arsenal = arsenal;
        }

        public Arsenal GetArsenal(HashList<Shot> shotHandle)
        {
            return arsenal.Copy(shotHandle);
        }
    }
}
