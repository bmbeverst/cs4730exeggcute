using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.roster
{
    class RosterEntry
    {
        public Model Surface { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public NewArsenal Arsenal { get; protected set; }

        public RosterEntry(Model model,
                           BehaviorScript behavior, 
                           NewArsenal arsenal)
        {
            this.Surface = model;
            this.Behavior = behavior;
            this.Arsenal = arsenal;
        }
    }
}
