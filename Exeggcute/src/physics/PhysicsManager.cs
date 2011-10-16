using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.physics
{
    class PhysicsManager
    {
        public Vector3 GlobalGravity { get; protected set; }
        protected List<Force> forces = new List<Force>();

        public PhysicsManager()
        {
            GlobalGravity = new Vector3(0, 0, -1);
        }

        public void RegisterForce(Force force)
        {
            forces.Add(force);
        }

        public void Affect(HashList<PlanarEntity3D> entities)
        {
            foreach (PlanarEntity3D entity in entities.GetKeys())
            {
                Vector3 totalForce = GetForceSum(entity.Position, entity.Mass);
                entity.Influence(totalForce + GlobalGravity);
            }
        }

        /// <summary>
        /// We assume that force application is a linear operator.
        /// </summary>
        public Vector3 GetForceSum(Vector3 pos, float mass)
        {
            Vector3 totalForce = Vector3.Zero;
            for (int i = forces.Count - 1; i >= 0 ; i -= 1)
            {
                Force current = forces[i];
                totalForce += current.GetMagnitude(pos, mass);
                if (current.IsDestroyed())
                {
                    forces.RemoveAt(i);
                }
            }
            return totalForce + GlobalGravity;
        }
    }
}
