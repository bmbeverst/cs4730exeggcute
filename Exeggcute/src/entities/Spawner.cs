using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A spawner is an entity which spawns shots. This is separate from the
    /// entity which is shooting because it may have its own movement/behavior
    /// script.
    /// </summary>
    class Spawner : ScriptedEntity
    {
        int timer = 0;
        bool active;
        int shotActionPtr = 0;
        Shot shot;
        HashList<Shot> shotListHandle;
        Mover mover;
        public Spawner(ArsenalEntry arsenalEntry, HashList<Shot> shotListHandle)
            : base(arsenalEntry.Spawn)
        {
            this.shot = new Shot(arsenalEntry);
            this.shotListHandle = shotListHandle;
            this.active = true;
            this.mover = new Mover(arsenalEntry.Behavior);
        }

        /*
         * TODO/FIXME: make spawners Move and MoveRel and MoveTo commands relative to parent!!!
         * 
         */
        public void Update(Vector3 parentPos, float parentAngle)
        {
            mover.Update(parentPos, parentAngle);
            Position = mover.Position;
            
            if (active)
            {
                
                if (timer > 0)
                {
                    timer -= 1;
                }
                else if (timer == 0)
                {
                    active = false;
                }
            }
            base.Update();
        }

        public void SpawnFor(int duration)
        {
            active = true;
            timer = duration;
        }

        public void Stop()
        {
            timer = 0;
            active = false;
        }

        public override void Process(SpawnAction spawn)
        {
            float angle = spawn.Angle + Angle;
            float distance = spawn.Distance;
            float x = distance * FastTrig.Cos(angle);
            float y = distance * FastTrig.Sin(angle);
            Z = 0;
            Vector3 pos = Position + new Vector3(x, y, 0);
            //Util.Die("{0}", pos);
            Shot cloned = shot.Clone(pos, angle);
            shotListHandle.Add(cloned);
            actionPtr += 1;
        }






        /*public EntityArgs Args { get; protected set; }
        public Vector3 PosOffset { get; protected set; }
        public float AngleOffset { get; protected set; }

        protected Arsenal arsenal;

        public Spawner(ScriptName script, ArsenalName arsenalName, Vector3 posOffset, float angleOffset, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            PosOffset = posOffset;
            AngleOffset = angleOffset;
            this.arsenal = ArsenalBank.Get(arsenalName);
        }

        public Spawner(ScriptName script, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            this.arsenal = ArsenalBank.Get(arsenalName);
        }

        public override void Process(SpawnAction spawn)
        {
            EntityArgs args = spawn.Args;
            float angle = args.AngleHeading + Angle;
            Vector3 pos = Position + args.SpawnPosition;
            Shot cloned = arsenal.Clone(spawn.ID, pos, angle);
            shotListHandle.Add(cloned);
            actionPtr += 1;
        }
        public void SetParams(Vector3 pos, float angle)
        {
            PosOffset = pos;
            AngleOffset = angle;
        }

        public void Follow(CommandEntity parent, bool lockPos, bool lockAngle)
        {
            if (lockPos)
                Position = parent.Position + PosOffset;

            if (lockAngle)
                Angle = parent.Angle + AngleOffset;

        }*/

        public Spawner Copy()
        {
            //lol FIXME
            return this;
        }

        public void AttachShotHandle(HashList<Shot> shotListHandle)
        {
            this.shotListHandle = shotListHandle;
        }
    }
}
