using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// TODO: process commands until a wait command is found
    /// </summary>
    abstract class CommandEntity : PlanarEntity3D
    {
        /// <summary>
        /// If the entity is done, it should be removed from the world.
        /// </summary>
        public bool IsDone { get; protected set; }
        public CommandEntity Parent { get; protected set; }
        protected ActionList actionList;

        protected HashList<Shot> ShotList;
        protected Arsenal arsenal;
        protected Spawner spawner;

        public int Health { get; protected set; }

        private int p;
        protected int cmdPtr
        {
            get { return p; }
            set { p = value % actionList.Count; }
        }

        public bool IsShooting { get; protected set; }

        /// <summary>
        /// keeps track of how long a command has been processed before
        /// not applicable for all commands 
        /// </summary>
        protected int counter = 0;

        /// <summary>
        /// <para>Initializes the command entity for use as an enemy/player/boss 
        /// with:</para>
        /// <para> - An action script</para>
        /// <para> - A model</para>
        /// <para> - An arsenal</para>
        /// </summary>
        public CommandEntity(ModelName modelName, 
                             ScriptName scriptName, 
                             ArsenalName arsenalName, 
                             ScriptName spawnerName, 
                             HashList<Shot> shotList)
            : base(modelName, Engine.Jail)
        {
            Health = 100;
            ShotList = shotList;
            actionList = ScriptBank.Get(scriptName);
            this.arsenal = ArsenalBank.Get(arsenalName);
            this.spawner = new Spawner(spawnerName, arsenalName, shotList);
        }

        /// <summary>
        /// <para>Initializes the command entity for use as a spawner with:</para>
        /// <para> - An action script</para>
        /// <para> - An arsenal</para>
        /// <para> - No model</para>
        /// </summary>
        public CommandEntity(ScriptName script, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(Engine.Jail)
        {
            ShotList = shotList;
            actionList = ScriptBank.Get(script);
            this.arsenal = ArsenalBank.Get(arsenalName);
        }

        /// <summary>
        /// <para>Initializes the command entity for use as a shot/collectable with:</para>
        /// <para> - A action script</para>
        /// <para> - A model</para>
        /// <para> - No arsenal</para>
        /// </summary>
        public CommandEntity(ModelName model, ScriptName script)
            : base(model, Engine.Jail)
        {
            ShotList = null;
            actionList = ScriptBank.Get(script);
            this.arsenal = null;
        }

        public void Reset()
        {
            this.counter = 0;
            this.cmdPtr = 0;
        }

        public virtual void Process(ActionBase cmd)
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
            // FIXME: 
            // seems excessively defensive. Also might be expensive
            // for thousands of objects?
            if (actionList == null || actionList.Count == 0) return;
            ///////////yuck//////////////////
            for (int i = cmdPtr; i < actionList.Count; i += 1)
            {
                ActionBase current = actionList[i];
                current.Process(this);
                if (current is WaitAction || actionList == null)
                {
                    break;
                }
            }
            /////////////////////////////////////////////
            //ActionBase current = actionList[cmdPtr];
            //current.Process(this);
            
        }


        public virtual void Process(MoveAction move)
        {
            Speed = move.Speed;
            LinearAccel = move.LinearAccel;
            AngularVelocity = move.AngularVelocity;
            AngularAccel = move.AngularAccel;
            cmdPtr += 1;
        }

        public virtual void Process(ShootAction shoot)
        {
            IsShooting = !IsShooting;
            cmdPtr += 1;

        }

        public virtual void Process(MoveToAction moveTo)
        {
            Vector3 start = Position;
            Vector3 target = moveTo.Destination;
            doSmoothTransition(start, target, moveTo.Duration);
            cmdPtr += 1;
        }

        public virtual void Process(MoveRelativeAction moveRel)
        {
            Vector3 start = Position;
            Vector3 target = start + moveRel.Displacement;
            doSmoothTransition(start, target, moveRel.Duration);
            cmdPtr += 1;
        }

        protected void doSmoothTransition(Vector3 start, Vector3 target, int duration)
        {
            float distance = Vector3.Distance(start, target);
            float speed = (distance / (duration - 1)) * 2;
            float angle = FastTrig.Atan2(target.Y - start.Y, target.X - start.X);
            float linearAccel = -(speed / duration);
            Speed = speed;
            Angle = angle;
            LinearAccel = linearAccel;
        }

        public virtual void Process(VanishAction vanish)
        {
            IsDone = true;
            cmdPtr += 1;
        }

        public virtual void Process(SpawnAction spawn)
        {
            EntityArgs args = spawn.Args;
            float angle = args.AngleHeading + Angle;
            Vector3 pos = Position + args.SpawnPosition;
            Shot cloned = arsenal.Clone(spawn.ID, pos, angle);
            ShotList.Add(cloned);
            cmdPtr += 1;
        }

        public virtual void Process(EndAction end)
        {
            //do nothing!
        }

        public virtual void Process(SetAction set)
        {
            Position = set.Position;
            cmdPtr += 1;
        }

        public virtual void Process(AimAction aim)
        {
            Angle = aim.Angle;
            cmdPtr += 1;
        }

        public virtual void Process(WaitAction wait)
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

        public virtual void Process(StopAction stop)
        {
            Speed = 0;
            Angle = 0;
            LinearAccel = 0;
            AngularAccel = 0;
            AngularVelocity = 0;
            VelocityZ = 0;
            cmdPtr += 1;
        }

        public void Collide(Shot shot)
        {
            Health -= shot.Damage;
        }

        public override void Update()
        {
            ProcessActions();
            if (spawner != null && IsShooting)
            {
                spawner.SetPosition(Position);
                spawner.Update();
            }
            base.Update();
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
        }


    }
}
