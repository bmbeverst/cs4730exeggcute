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
        public float TerminalSpeed { get; protected set; }

        public PhysicsManager()
        {
            TerminalSpeed = 1;
            GlobalGravity = new Vector3(0, 0, -0.1633333f);
        }

        public void RegisterForce(Force force)
        {
            forces.Add(force);
        }

        public void Affect<TEntity>(HashList<TEntity> entities) where TEntity : PlanarEntity3D
        {
            foreach (TEntity entity in entities.GetKeys())
            {
                Vector3 totalForce = GetForceSum(entity.Position, entity.Mass);
                entity.Influence(totalForce + GlobalGravity, TerminalSpeed);
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
