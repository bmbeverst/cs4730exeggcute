using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.arsenal;
using Microsoft.Xna.Framework.Graphics;

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
        Model arrow;
        Model debugModel;
        ArsenalEntry arsenalEntry;
        public Mover mover { get; protected set; }
        public Vector3 ParentPos { get; protected set; }
        public float ParentAngle { get; protected set; }
        public Spawner(ArsenalEntry arsenalEntry, HashList<Shot> shotListHandle)
            : base(arsenalEntry.Spawn)
        {
            this.arsenalEntry = arsenalEntry;
            this.shot = new Shot(arsenalEntry);
            this.shotListHandle = shotListHandle;
            this.active = true;
            this.mover = new Mover(arsenalEntry.Behavior);
            this.Scale = 1.0f;
            this.arrow = ModelBank.Get("arrow");
            this.debugModel = ModelBank.Get("XNAface");
        }

        float debugAngle;
        Vector3 debugPosition;
        Vector3 arrowPosition;

        public void UpdateMover(Vector3 parentPos, float parentAngle)
        {
            ParentPos = parentPos;
            ParentAngle = parentAngle;
            mover.Update(parentPos, parentAngle);
            Position = mover.Position;


            Vector3 moverPos = mover.Position + mover.ParentPosition;
            Vector3 difference = ParentPos - moverPos;
            if (difference.Equals(Vector3.Zero)) return;
            debugAngle = FastTrig.Atan2(difference.Y, difference.X);
            debugPosition = moverPos;
            arrowPosition = Util.AngleToVector3(debugAngle) + debugPosition;
        }
        /*
         * TODO/FIXME: make spawners Move and MoveRel and MoveTo commands relative to parent!!!
         * 
         */
        public void Update(Vector3 parentPos, float parentAngle)
        {

            UpdateMover(parentPos, parentAngle);
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
            Z = 0;
            Vector3 pos = mover.ParentPosition + mover.Position;// +new Vector3(x, y, 0);
            float angle;
            if (spawn.Type == AngleType.Abs)
            {
                angle = spawn.Angle.Value;
            }
            else //relative
            {
                if (mover.IsAiming)
                {
                    angle = mover.AimAngle + spawn.Angle.Value;
                }
                else
                {
                    angle = mover.Angle + spawn.Angle.Value;
                }
                
            }
            Shot cloned = shot.Clone(pos, angle);
            shotListHandle.Add(cloned);
            actionPtr += 1;
        }

        public void AttachShotHandle(HashList<Shot> shotListHandle)
        {
            this.shotListHandle = shotListHandle;
        }

        /// <summary>
        /// FOR DEBUG ONLY
        /// </summary>
        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            
            Matrix[] transforms = new Matrix[arrow.Bones.Count];
            arrow.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in arrow.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    //FIXME: absolutely no reason to do this every frame
                    currentEffect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(0.5f, 0.1f, 0.1f) *
                        Matrix.CreateRotationZ(debugAngle) *
                        Matrix.CreateTranslation(arrowPosition);
                    currentEffect.View = view;
                    currentEffect.Projection = projection;
                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in debugModel.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    //FIXME: absolutely no reason to do this every frame
                    currentEffect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(0.5f, 0.5f, 0.5f) *
                        Matrix.CreateRotationZ(debugAngle) *
                        Matrix.CreateTranslation(debugPosition);
                    currentEffect.View = view;
                    currentEffect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
