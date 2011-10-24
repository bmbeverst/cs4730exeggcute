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
    abstract class ScriptedEntity : PlanarEntity3D
    {
        protected Script script;
        public int actionPtr { get; protected set; }

        public bool IsAiming { get; protected set; }

        public int Health { get; protected set; }

        /// <summary>
        /// keeps track of how long a command has been processed before
        /// not applicable for all commands 
        /// </summary>
        protected int counter = 0;


        /// <summary>
        /// <para>
        /// Initializes the command entity for use as an enemy/player/boss 
        /// with:
        /// </para>
        /// </summary>
        public ScriptedEntity(Model model, Texture2D texture, BehaviorScript behavior)
            : base(model, texture, Engine.Jail)
        {
            this.Health = 100;
            this.script = behavior;
        }

        /// <summary>
        /// For use with a spawner 
        /// </summary>
        public ScriptedEntity(SpawnScript spawn)
            : base(Engine.Jail)
        {
            this.script = spawn;
        }

        public ScriptedEntity(BehaviorScript behavior)
            : base(Engine.Jail)
        {
            this.script = behavior;
        }

        /// <summary>
        /// <para>Initializes the command entity for use as a shot/collectable with:</para>
        /// <para> - A action script</para>
        /// <para> - A model</para>
        /// <para> - No arsenal</para>
        /// </summary>
        public ScriptedEntity(Model model, Texture2D texture, TrajectoryScript trajectory)
            : base(model, texture, Engine.Jail)
        {
            this.script = trajectory;
        }


        public void Reset()
        {
            this.counter = 0;
            this.actionPtr = 0;
        }

        public virtual void Process(ActionBase cmd)
        {
            string error = @"
            Fatal Error:
            This is the default ActionBase handler. This was called
            because there was no handler in CommandEntity for the
            type {0} in script {1}";
            string message = string.Format(error, cmd.GetType(), script);
            throw new NotImplementedException(message);
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
            if (script == null || script.Count == 0) return;
            ///////////yuck//////////////////
            for (int i = actionPtr; i < script.Count; i += 1)
            {
                ActionBase current = script[i];
                current.Process(this);
                if (current is WaitAction || script == null)
                {
                    break;
                }
            }
            /////////////////////////////////////////////
            //ActionBase current = actionList[cmdPtr];
            //current.Process(this);
            
        }
        public virtual void Process(DeleteAction delete)
        {
            QueueDelete();
            //actionPtr = -1;
        }

        public virtual void Process(MoveAction move)
        {
            Speed = move.Speed.Value;
            LinearAccel = move.LinearAccel.Value;
            AngularVelocity = move.AngularVelocity.Value;
            AngularAccel = move.AngularAccel.Value;
            actionPtr += 1;
        }

        public virtual void Process(ShootAction shoot)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void Process(MoveAbsAction moveTo)
        {
            Vector3 start = Position;
            Vector3 target = moveTo.Destination.Vector;
            doSmoothTransition(start, target, moveTo.Duration);
            actionPtr += 1;
        }

        public virtual void Process(MoveRelativeAction moveRel)
        {
            Vector3 start = Position;
            Vector3 target = start + moveRel.Displacement.Vector;
            doSmoothTransition(start, target, moveRel.Duration);
            actionPtr += 1;
        }

        public virtual void Process(AimPlayerAction aim)
        {
            AimAngle = Util.AimAt(Position, Level.player.Position);
            IsAiming = true;
            actionPtr += 1;
        }

        protected void doSmoothTransition(Vector3 start, Vector3 target, int duration)
        {
            float distance = Vector3.Distance(start, target);
            float speed = (distance / (duration - 1)) * 2;
            float dx = target.X - start.X;
            float dy = target.Y - start.Y;
            if (dy == 0 && dx == 0) return;
            float angle = FastTrig.Atan2(dy, dx);
            float linearAccel = -(speed / duration);
            Speed = speed;
            Angle = angle;
            LinearAccel = linearAccel;
        }

        public virtual void Process(SpawnAction spawn)
        {
            // This is nicer than forcing all subclasses to implement this
            // by making it abstract.
            // Only the spawner class should be using this action type.
            throw new SubclassShouldImplementError();
        }

        public virtual void Process(AimAction aim)
        {
            Angle = aim.Angle.Value;
            actionPtr += 1;
        }

        public virtual void Process(WaitAction wait)
        {
            if (counter >= wait.Duration)
            {
                actionPtr += 1;
                counter = 0;
            }
            else
            {
                
                counter += 1;
            }
        }

        public virtual void Process(SetParamAction setparam)
        {
            param[setparam.ParamIndex] = setparam.Value.Value;
            actionPtr += 1;
        }

        public virtual void Process(StopAction stop)
        {
            Speed = 0;
            LinearAccel = 0;
            AngularAccel = 0;
            AngularVelocity = 0;
            VelocityZ = 0;
            actionPtr += 1;
        }

        public virtual void Process(LoopAction loop)
        {
            actionPtr = loop.Pointer;
        }

        public void Collide(Shot shot)
        {
            Health -= shot.Damage;
        }

        public override void Update()
        {
            ProcessActions();
            base.Update();
        }


    }
}
