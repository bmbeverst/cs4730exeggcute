using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    abstract class PlanarEntity3D : Entity3D
    {
        public Vector3 PrevPosition { get; protected set; }
        public float Mass { get; protected set; }
        public float Angle { get; protected set; }
        public float Speed { get; protected set; }
        public float AngularVelocity { get; protected set; }
        public float LinearAccel { get; protected set; }
        public float AngularAccel { get; protected set; }
        public float VelocityZ { get; protected set; }

        

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

        public Vector2 Position2D
        {
            get { return new Vector2(Position.X, Position.Y); }
        }

        public PlanarEntity3D(Model model, Vector3 pos)
            : base(model, pos)
        {
            Mass = 1000.0f;
        }

        public PlanarEntity3D(Vector3 pos)
            : base (pos)
        {
            Mass = 1;
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
            /*if (accel.Equals(Vector3.Zero)) return;
            //Console.WriteLine("accel:{0}", accel);
            float x = accel.X + vx;
            float y = accel.Y + vy;
            Vector2 newVelocity = new Vector2(x, y);
            Speed = newVelocity.Length();
            //Console.WriteLine("{0} ({1},{2})",Speed, x, y);
            //Console.Write("OLD ({0}, ");
            Angle = FastTrig.Atan2(newVelocity.Y, newVelocity.X);
            //Console.WriteLine("{0}) NEW", Angle);*/
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
            Angle += AngularVelocity;

            // Process velocities
            Angle += AngularVelocity;
            X += vx;
            Y += vy;
            Z += VelocityZ;
        }
        
    }
}
