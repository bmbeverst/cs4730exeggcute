using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalEntry
    {
        public Model Surface { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public TrajectoryScript Trajectory { get; protected set; }
        public SpawnScript Spawn { get; protected set; }

        public ArsenalEntry(Model model, BehaviorScript behavior, SpawnScript spawn, TrajectoryScript trajectory)
        {
            this.Surface = model;
            this.Behavior = behavior;
            this.Spawn = spawn;
            this.Trajectory = trajectory;
        }
    }
}
