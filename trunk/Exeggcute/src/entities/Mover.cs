using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;

namespace Exeggcute.src.entities
{
    class Mover : ScriptedEntity
    {
        public Vector3 ParentPosition { get; protected set; }
        public float ParentAngle { get; protected set; }

        public Mover(BehaviorScript moveBehavior)
            : base(moveBehavior)
        {
            Position = new Vector3(0, 0, 0);
        }

        public void Update(Vector3 parentPos, float parentAngle)
        {
            this.ParentPosition = parentPos;
            this.ParentAngle = parentAngle;
            if (IsAiming) AimAtPlayer();
            base.Update();
        }

        public override void Process(MoveRelAction moveRel)
        {
            Vector3 start = ParentPosition + Position;
            Vector3 target = start + moveRel.Displacement.Vector3;
            doSmoothTransition(start, target, moveRel.Duration);
            ActionPtr += 1;
        }
        public override void Process(AimPlayerAction aim)
        {
            AimAngle = Util.AimAt(Position, Level.player.Position);
            IsAiming = true;
        }

        public void AimAtPlayer()
        {
            AimAngle = Util.AimAt(Position, Level.player.Position);
        }
    }
}
