using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework.Graphics;

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
            base.Update();
        }

        public override void Process(MoveRelativeAction moveRel)
        {
            Vector3 start = ParentPosition + Position;
            Vector3 target = start + moveRel.Displacement.Vector;
            doSmoothTransition(start, target, moveRel.Duration);
            actionPtr += 1;
        }

        /*public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            Matrix[] transforms = new Matrix[Surface.Bones.Count];
            Surface.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in Surface.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    //FIXME: absolutely no reason to do this every frame
                    currentEffect.World = 
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationZ(ParentAngle) *
                        Matrix.CreateTranslation(ParentPosition);
                    currentEffect.View = view;
                    currentEffect.Projection = projection;
                }
                mesh.Draw();
            }
        
        }*/
    }
}
