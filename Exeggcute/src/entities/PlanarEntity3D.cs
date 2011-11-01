using System;
using System.Collections.Generic;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    abstract class PlanarEntity3D : Entity3D
    {
        public Model Surface { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public Vector3 PrevPosition { get; protected set; }
        protected float[] param = new float[16];
        public int ParamCount { get { return param.Length; } }
        public static Dictionary<string, int> ParamMap;
        public static List<string> validParameters = new List<string>();

        static PlanarEntity3D()
        {
            Dictionary<string, int> temp = new  Dictionary<string, int> 
            {
                { "PositionX",         0 },
                { "PositionY",         1 },
                { "PositionZ",         2 },
                { "Mass",              3 },
                { "Angle",             4 },
                { "Speed",             5 },
                { "VelocityZ",         6 },
                { "AngularVelocity",   7 },
                { "LinearAccel",       8 },
                { "AngularAccel",      9 },
                { "FacingX",          10 },
                { "FacingY",          11 },
                { "FacingZ",          12 },
                { "AimAngle",         13 },
                { "AimAngleVelocity", 14 },
                { "AimAngleAccel",    15 }
            };

            ParamMap = new Dictionary<string, int>();
            foreach (var pair in temp)
            {
                string param = pair.Key;
                ParamMap[param.ToLower()] = pair.Value;
                validParameters.Add(param);
            }
    
        }

        public override Vector3 Position
        {
            get { return new Vector3(param[0], param[1], param[2]); }
            protected set
            {
                param[0] = value.X;
                param[1] = value.Y;
                param[2] = value.Z;
            }
        }
        public virtual float X
        {
            get { return param[0]; }
            protected set { param[0] = value; }
        }

        public virtual float Y
        {
            get { return param[1]; }
            protected set { param[1] = value; }
        }

        public virtual float Z
        {
            get { return param[2]; }
            protected set { param[2] = value; }
        }
        
        public float Mass 
        {
            get { return param[3]; }
            protected set { param[3] = value; }
        }

        public float Angle 
        {
            get { return param[4]; }
            protected set { param[4] = value; }
        }

        public float Speed 
        {
            get { return param[5]; }
            protected set { param[5] = value; }
        }
        public float VelocityZ
        {
            get { return param[6]; }
            protected set { param[6] = value; }
        }
        public float AngularVelocity 
        {
            get { return param[7]; }
            protected set { param[7] = value; }
        }

        public float LinearAccel 
        {
            get { return param[8]; }
            protected set { param[8] = value; }
        }

        public float AngularAccel 
        {
            get { return param[9]; }
            protected set { param[9] = value; } 
        }

        public Vector3 Facing
        {
            get { return new Vector3(param[10], param[11], param[12]); }
            protected set 
            { 
                param[10] = value.X; 
                param[11] = value.Y; 
                param[12] = value.Z; 
            }
        }

        public float AimAngle
        {
            get { return param[13]; }
            protected set { param[13] = value; }
        }

        public float AimAngleVelocity
        {
            get { return param[14]; }
            protected set { param[14] = value; }
        }

        public float AimAngleAccel
        {
            get { return param[15]; }
            protected set { param[15] = value; }
        }



        public float Scale { get; protected set; }

        /*public float ModelRotX { get; protected set; }
        public float ModelRotY { get; protected set; }
        public float ModelRotZ { get; protected set; }*/
        public Vector3 ModelRotation;
        private float vx
        {
            get { return Speed * FastTrig.Cos(Angle); }
        }
        private float vy
        {
            get { return Speed * FastTrig.Sin(Angle); }
        }
        public Vector3 Velocity
        {
            //FIXME: cache!
            get { return new Vector3(vx, vy, 0); }
        }
        public Matrix BaseMatrix { get; protected set; }
        public Vector2 Position2D
        {
            get { return new Vector2(Position.X, Position.Y); }
        }
        public Vector3 DegRotation;
        public PlanarEntity3D(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, Vector3 pos)
            : base(pos, radius)
        {
            this.Surface = model;
            this.Texture = texture;
            this.Scale = scale;
            this.DegRotation = rotation;
            this.ModelRotation = DegRotation * FastTrig.degreesToRadians;
            this.Mass = 1000.0f;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Assets.Effect["light0"];
                }
            }

            BaseMatrix =
                Matrix.CreateScale(Scale) * 
                Matrix.CreateRotationX(ModelRotation.X) *
                Matrix.CreateRotationY(ModelRotation.Y) *
                Matrix.CreateRotationZ(ModelRotation.Z);
            
        }

        public PlanarEntity3D(Vector3 pos)
            : base (pos)
        {
            this.Mass = 1;
        }

        /// <summary>
        /// Used to allow subclasses to implement ILoadable
        /// </summary>
        protected PlanarEntity3D()
            :base(Vector3.Zero)
        {
            this.Mass = 1;
        }

        /// <summary>
        /// Tells whether the entity is in a rectangle, usually used to 
        /// tell if the entity is offscreen or not
        /// </summary>
        public bool ContainedIn(Rectangle rect)
        {
            return rect.Contains((int)X, (int)Y);
        }

        public void QueueDelete()
        {
            IsTrash = true;
        }

        public void SetVelocityZ(float value)
        {
            VelocityZ = value;
        }

        public void SetAngle(float angle)
        {
            Angle = angle;
        }

        public virtual void Influence(Vector3 accel, float terminal)
        {

            VelocityZ += accel.Z;
            VelocityZ = Math.Min(VelocityZ, terminal);
            
        }

        public override void Update()
        {
            base.Update();
            PrevPosition = Position;
            ProcessPhysics();
        }

        protected virtual void ProcessPhysics()
        {

            // Process accelerations
            Speed += LinearAccel;
            AngularVelocity += AngularAccel;

            // Process velocities
            Angle += AngularVelocity;
            X += vx;
            Y += vy;
            Z += VelocityZ;
        }
        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            //FIXME subclass!
            if (Surface == null) return;
            Matrix[] transforms = new Matrix[Surface.Bones.Count];
            Surface.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in Surface.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    //FIXME: absolutely no reason to do this every frame
                    /*currentEffect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationZ(Angle + MathHelper.PiOver2) *
                        Matrix.CreateTranslation(Position);
                    currentEffect.View = view;
                    currentEffect.Projection = projection;*/

                    Matrix world = transforms[mesh.ParentBone.Index] *
                        /*Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationX(ModelRotation.X) *
                        Matrix.CreateRotationY(ModelRotation.Y) **/
                        BaseMatrix *
                        Matrix.CreateRotationZ(Angle /*+ ModelRotation.Z*/) *
                        Matrix.CreateTranslation(Position);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(world);
                    currentEffect.Parameters["xView"].SetValue(view);
                    currentEffect.Parameters["xProjection"].SetValue(projection);
                    currentEffect.Parameters["xTexture"].SetValue(Texture);
                  
                }
                mesh.Draw();
            }

        }
        
    }
}
