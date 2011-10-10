using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entity;

namespace Exeggcute.src
{
    class CollisionManager
    {
        public void Update()
        {

        }

        public void Collide(List<Entity3D> entities)
        {
            for (int i = 0; i < entities.Count; i += 1)
            {
                for (int j = i + 1; j < entities.Count; j += 1)
                {
                    if (entities[i].Hitbox.Intersects(entities[j].Hitbox))
                    {

                    }
                }
            }
        }




    }
}
