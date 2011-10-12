using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using System.Collections.ObjectModel;

namespace Exeggcute.src.entities
{

    class CommandEntity : PlanarEntity3D
    {
        /// <summary>
        /// If the entity is done, it should be removed from the world.
        /// </summary>
        public bool IsDone { get; protected set; }
        public CommandEntity Parent { get; protected set; }
        private ActionList actionList;
        private List<ShotSpawner> spawners = new List<ShotSpawner>();
        
        private int spawnPtr;
        private bool shooting;

        private int p;
        private int cmdPtr
        {
            get { return p; }
            set { p = value % actionList.Count; }
        }

        /// <summary>
        /// keeps track of how long a command has been processed before
        /// not applicable for all commands 
        /// </summary>
        private int counter = 0;

        public CommandEntity(ModelName name)
            : base(name, new Vector3(0,0,0))
        {
            actionList = ScriptBank.Get(ScriptName.test);
        }

        public void Process(ActionBase cmd)
        {
            string error = @"
            This is the default ActionBase handler. This was called
            because there was no handler in CommandEntity for the
            type {0}";
            Util.Warn(error, cmd.GetType());
            throw new NotImplementedException();
        }


        /// <summary>
        /// In order to handle the different subtypes of ActionBase, we employ 
        /// "double dispatch". Since method overloading is resolved at
        /// compile time in C#, we must call the Process method on our command
        /// and then immediately call the corresponding Process method in
        /// this class. This will cause the proper overload to be called.
        /// 
        /// When you add a new ActionBase subclass, add a Process(NewActionType)
        /// overload.
        /// </summary>
        public void ProcessActions()
        {
            if (actionList.Count == 0) return;
            ActionBase current = actionList[cmdPtr];
            current.Process(this);
            
        }


        public void Process(MoveAction move)
        {
            Speed = move.Speed;
            Angle = move.Angle;
            LinearAccel = move.LinearAccel;
            cmdPtr += 1;
        }

        public void Process(VanishAction vanish)
        {
            IsDone = true;
            if (Parent != null)
            {
                throw new NotImplementedException();
            }
            cmdPtr += 1;
        }

        public void Process(SetAction set)
        {
            Position = set.Position;
            cmdPtr += 1;
        }

        public void Process(WaitAction wait)
        {
            if (counter >= wait.Duration)
            {
                cmdPtr += 1;
                counter = 0;
            }
            else
            {
                counter += 1;
            }
        }

        public void Process(ShootAction shoot)
        {
            if (shoot.Start) spawnPtr = shoot.ShotIndex;
            shooting = shoot.Start;
            cmdPtr += 1;
        }

        public void Process(StopAction stop)
        {
            Speed = 0;
            Angle = 0;
            LinearAccel = 0;
            AngularAccel = 0;
            AngularVelocity = 0;
            VelocityZ = 0;
            cmdPtr += 1;
        }

        public override void Update()
        {
            ProcessActions();

            if (shooting)
            {
                spawners[spawnPtr].TrySpawnAt(Position);
            }

            base.Update();
        }



    }
}
