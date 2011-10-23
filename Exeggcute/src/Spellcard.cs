﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;

namespace Exeggcute.src
{
    class Spellcard
    {
        public BehaviorScript Behavior { get; protected set; }
        public Arsenal Attack { get; protected set; }
        public Timer TimeLimit { get; protected set; }
        public ItemBatch HeldItems { get; protected set; }
        public readonly int Duration;
        public readonly int Health;
        public readonly string Name;

        public Spellcard(BehaviorScript behavior, Arsenal arsenal, ItemBatch items, int duration, int health, string name)
        {
            this.Behavior = behavior;
            this.Attack = arsenal;
            this.Duration = duration;
            this.TimeLimit = new Timer(duration);
            this.HeldItems = items;
            this.Health = health;
            this.Name = name;
        }

        

        

    }
}
