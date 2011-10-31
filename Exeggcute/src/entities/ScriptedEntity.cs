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
        protected ScriptInstance script;

        /// <summary>
        /// keeps track of how long a command has been processed before
        /// not applicable for all commands 
        /// </summary>
        public int WaitCounter { get; protected set; }

        /// <summary>
        /// po
        /// </summary>
        public int ActionPtr { get; protected set; }

        public bool IsAiming { get; protected set; }

        public int Health { get; protected set; }
        public int Defence { get; protected set; }
        


        /// <summary>
        /// <para>
        /// Initializes the command entity for use as an enemy/player/boss 
        /// with:
        /// </para>
        /// </summary>
        public ScriptedEntity(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, BehaviorScript behavior)
            : base(model, texture, scale, radius, rotation, Engine.Jail)
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
        public ScriptedEntity(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, TrajectoryScript trajectory)
            : base(model, texture, scale, radius, rotation, Engine.Jail)
        {
            this.script = trajectory;
        }

        /// <summary>
        /// Used to allow subclasses to implement ILoadable
        /// </summary>
        protected ScriptedEntity()
        {

        }

        public virtual void Reset()
        {
            this.WaitCounter = 0;
            this.ActionPtr = 0;
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

        public virtual void Process(UpgradeAction upgrade)
        {
            throw new SubclassShouldImplementError("For player only");
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
            for (int i = ActionPtr; i < script.Count; i += 1)
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

        public virtual void Process(SoundAction sound)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void Process(MoveAction move)
        {
            Speed = move.Speed.Value;
            LinearAccel = move.LinearAccel.Value;
            AngularVelocity = move.AngularVelocity.Value;
            AngularAccel = move.AngularAccel.Value;
            ActionPtr += 1;
        }

        public virtual void Process(ShootAction shoot)
        {
            throw new SubclassShouldImplementError();
        }

        public virtual void Process(MoveAbsAction moveTo)
        {
            Vector3 start = Position;
            Vector3 target = moveTo.Destination.Vector3;
            doSmoothTransition(start, target, moveTo.Duration);
            ActionPtr += 1;
        }

        public virtual void Process(MoveRelAction moveRel)
        {
            Vector3 start = Position;
            Vector3 target = start + moveRel.Displacement.Vector3;
            doSmoothTransition(start, target, moveRel.Duration);
            ActionPtr += 1;
        }

        public virtual void Process(AimPlayerAction aim)
        {
            AimAngle = Util.AimAt(Position, Level.player.Position);
            IsAiming = true;
            ActionPtr += 1;
        }

        protected void doSmoothTransition(Vector3 start, Vector3 target, int duration)
        {
            float distance = Vector3.Distance(start, target);
            float speed = (distance / (duration - 1)) * 2;
            float dx = target.X - start.X;
            float dy = target.Y - start.Y;
            if (dy == 0 && dx == 0)
            {
                Util.Warn("WARNING already at {0} from {1}", target, start);
                return;
            }
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

        public virtual void Process(WaitAction wait)
        {
            if (WaitCounter >= wait.Duration)
            {
                ActionPtr += 1;
                WaitCounter = 0;
            }
            else
            {
                
                WaitCounter += 1;
            }
        }

        public virtual void Process(SetParamAction setparam)
        {
            param[setparam.ParamIndex] = setparam.Value.Value;
            ActionPtr += 1;
        }

        public virtual void Process(StopAction stop)
        {
            Speed = 0;
            LinearAccel = 0;
            AngularAccel = 0;
            AngularVelocity = 0;
            VelocityZ = 0;
            ActionPtr += 1;
        }

        public virtual void Process(LoopAction loop)
        {
            ActionPtr = loop.Pointer;
        }

        public virtual void Collide(Shot shot)
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
