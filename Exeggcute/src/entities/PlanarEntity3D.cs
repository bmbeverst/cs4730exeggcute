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

        public PlanarEntity3D(ModelName modelName, Vector3 pos)
            : base(modelName, pos)
        {

        }

        public PlanarEntity3D(Vector3 pos)
            : base (pos)
        {

        }
        public virtual void Influence(Vector3 accel)
        {

        }
        public override void Update()
        {
            base.Update();
            PrevPosition = Position;

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
