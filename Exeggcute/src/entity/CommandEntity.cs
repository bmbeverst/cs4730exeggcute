using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class CommandEntity : PlanarEntity3D
    {
        private List<Command> commandList = new List<Command>();

        private int p;
        private int cmdPtr
        {
            get { return p; }
            set { p = value % commandList.Count; }
        }

        private int counter = 0;

        public CommandEntity(ModelName name, Vector3 pos)
            : base(name, pos)
        {

        }

        public void ProcessCommands()
        {
            if (commandList.Count == 0) return;
            Command current = commandList[cmdPtr];
            if (current.Type == CommandType.MoveTo)
            {

            }

            counter += 1;
            if (current.Duration <= counter)
            {
                cmdPtr += 1;
                counter = 0;
            }
        }

        public void CalculateHeading(Vector2 target, int duration)
        {
            /*  from HF
                distance = float(self.point_distance(position))
                time = 2*float(distance)/speed
                acceleration = -speed/float(time)
                self.speed = speed
                self.acceleration = acceleration */

            float distance = Vector2.Distance(Position2D, target);
            this.Speed = distance / duration;
            this.Angle = FastTrig.Atan2(target.Y - Position2D.Y, target.X - Position2D.X);
            this.LinearAccel = -Speed / duration;

        }

        public override void Update()
        {
            ProcessCommands();
            base.Update();
        }



    }
}
