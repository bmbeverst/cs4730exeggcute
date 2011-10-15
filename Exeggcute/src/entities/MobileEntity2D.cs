using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    abstract class MobileEntity2D : Entity2D
    {
        public Vector2 PrevPosition { get; protected set; }
        public float Angle { get; protected set; }
        public float Speed { get; protected set; }
        public float AngularVelocity { get; protected set; }
        public float LinearAccel { get; protected set; }
        public float AngularAccel { get; protected set; }
        private float vx
        {
            get { return Speed * FastTrig.Cos(Angle); }
        }
        private float vy
        {
            get { return Speed * FastTrig.Sin(Angle); }
        }
        public Vector2 Velocity
        {
            //FIXME: cache!
            get { return new Vector2(vx, vy); }
        }
        public MobileEntity2D(TextureName name, Vector2 pos)
            : base(name, pos)
        {

        }
        public override void Update()
        {
            /////////////////////////////////////////////////////////
            // We assume that base.Update does not alter Position in
            // a way that invalidates the following line
            base.Update();
            /////////////////////////////////////////////////////////
            PrevPosition = Position;

            // Process accelerations
            Speed += LinearAccel;
            Angle += AngularVelocity;

            // Process velocities
            Angle += AngularVelocity;
            Position += Velocity;
        }

        
        public override void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, Position, Angle);
        }




    }
}
