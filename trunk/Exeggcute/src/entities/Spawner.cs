﻿using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
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
        protected int timer = 0;
        protected bool active;
        protected int shotActionPtr = 0;
        protected Shot shot;

        protected Model arrow;
        protected Model debugModel;

        public Mover mover { get; protected set; }
        public Vector3 ParentPos { get; protected set; }
        public float ParentAngle { get; protected set; }

        protected RepeatedSound shotSound;



        public Spawner(Model model, 
                       Texture2D texture, 
                       float scale, 
                       float radius,
                       Vector3 rotation,
                       int damage, 
                       TrajectoryScript shotTrajectory, 
                       SpawnScript spawnScript, 
                       BehaviorScript moverScript, 
                       RepeatedSound shotSound)
            : base(spawnScript)
        {
            this.shot = new Shot(model, texture, scale, radius, rotation, shotTrajectory, damage);
            this.shotSound = shotSound;
            this.active = false;
            this.mover = new Mover(moverScript);
            this.Scale = 1.0f;
            this.arrow = Assets.Model["arrow"];
            this.debugModel = Assets.Model["XNAface"];
        }

        public void SetAlignment(Alignment alignment)
        {
            this.Alignment = alignment;
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
                base.Update();
                if (timer > 0)
                {
                    timer -= 1;
                }
                else if (timer == 0)
                {
                    active = false;
                }
            }
            
        }

        public void SpawnFor(int duration)
        {
            active = true;
            timer = duration;
        }

        public void Spawn()
        {
            active = true;
        }

        public void Stop()
        {
            timer = 0;
            active = false;
        }

        public override void Process(SoundAction sound)
        {
            shotSound.Play();
            ActionPtr += 1;
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
            World.AddShot(cloned, Alignment);
            ActionPtr += 1;
        }

        /// <summary>
        /// FOR DEBUG ONLY
        /// </summary>
        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            return;
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
