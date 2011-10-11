using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.commands;

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

        public CommandEntity(ModelName name)
            : base(name, new Vector3(0,0,0))
        {
            commandList = CommandListLoader.Load("test");
        }

        public void ProcessCommands()
        {
            if (commandList.Count == 0) return;
            Command current = commandList[cmdPtr];
            Console.WriteLine("type of command is {0}", current.GetType());
            if (current is MoveCommand)
            {
                MoveCommand move = (MoveCommand)current;
                Speed = move.Speed;
                Angle = move.Angle;
                LinearAccel = move.LinearAccel;
            }
            
            else if (current is ResetCommand)
            {
                Position = ((ResetCommand)current).Position;
                Speed =
                    Angle =
                    LinearAccel =
                    AngularAccel =
                    AngularVelocity =
                    VelocityZ = 0;

            }

            if (current is WaitCommand)
            {
                if (counter >= ((WaitCommand)current).Duration)
                {
                    cmdPtr += 1;
                    counter = 0;
                }
                else
                {

                    counter += 1;
                }

            }
            else
            {
                Console.WriteLine("PLUS {0}", commandList.Count);
                cmdPtr += 1;
            }
            
        }

        public override void Update()
        {
            ProcessCommands();
            base.Update();
        }



    }
}
