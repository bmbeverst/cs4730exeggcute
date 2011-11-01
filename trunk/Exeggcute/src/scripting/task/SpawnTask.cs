using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class SpawnTask : Task
    {
        static SpawnTask()
        {
            docs[typeof(SpawnTask)] = new Dictionary<Info,string> 
            {
                { Info.Syntax, "Spawn ID POS ANGLE" },
                { Info.Args, 
@"int ID         
    The index of the enemy to be spawned from the level's roster.
Float3     POS 
    The position at which to spawn the enemy.
FloatValue ANGLE   
    The angle to spawn the enemy with." },
                { Info.Description, "Spawns an enemy with the specified parameters from the roster."},
                { Info.Example, 
@"spawn 0 ([-10,10], 0, 0) 45
    Spawns the 0th enemy from the roster at a random position 
    between (-10,0,0) and (10,0,0)."}
            };
        }

        public int ID { get; protected set; }
        public Float3 Position { get; protected set; }
        public FloatValue Angle { get; protected set; }

        public SpawnTask(int id, Float3 pos, FloatValue angle)
        {
            this.ID = id;
            this.Position = pos;
            this.Angle = angle;
        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }


    }
}
