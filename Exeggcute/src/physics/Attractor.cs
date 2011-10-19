using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.physics
{
    /// <summary>
    /// Force is ((r->) * this.Mass * other.Mass) / d ^ (Exponent + 1)
    /// where d = |r->|
    ///          
    /// </summary>
    class Attractor : Force
    {
        public Vector3 Position { get; protected set; }

        public int Exponent { get; protected set; }
        public float Mass { get; protected set; }

        /// <summary>
        /// To avoid infinite accelerations very near to the singularity,
        /// we do not return any force when the entity is within DeadZone
        /// of the Attractor.
        /// </summary>
        public float DeadZone { get; protected set; }

        public Attractor(Vector3 pos, int exp, float mass, float deadZone)
        {
            Position = pos;
            Exponent = exp;
            Mass = mass;
            DeadZone = deadZone;
        }

        //FIXME: sign might be wrong on this one
        public virtual Vector3 GetMagnitude(Vector3 otherPos, float otherMass)
        {

            Vector3 r = otherPos - Position;
            double d = r.Length();
            if (d < DeadZone) return Vector3.Zero;
            float d_pow = (float)Math.Pow((double)r.Length(), (double)(Exponent + 1.0));
            Vector3 result  = ((Mass * otherMass * PhysicsManager.BigG) / d_pow) * r;
            Console.WriteLine(result);
            return -result;
        }

        public virtual bool IsDestroyed()
        {
            return false;
        }

        public void SetPosition(Vector3 newPosition)
        {
            Position = newPosition;
        }
    }
}
