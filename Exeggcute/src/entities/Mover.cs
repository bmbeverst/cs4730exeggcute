using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework;

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
            if (IsAiming) aimAtPlayer();
            base.Update();
        }

        public override void Process(MoveRelAction moveRel)
        {
            Vector3 start = ParentPosition + Position;
            Vector3 target = start + moveRel.Displacement.Vector3;
            doSmoothTransition(start, target, moveRel.Duration);
            ActionPtr += 1;
        }
    }
}
