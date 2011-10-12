using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src
{
    class CollisionManager
    {
        public void Update()
        {

        }

        public void Collide(Player3D player, List<Entity3D> enemies)
        {
            foreach (Entity3D enemy in enemies)
            {
                if (enemy.Hitbox.Intersects(player.Hitbox))
                {
                    
                }
            }
        }

        public void Collide(List<Shot> playerShots, List<Entity3D> enemies)
        {
            foreach (Shot shot in playerShots)
            {
                foreach (Entity3D enemy in enemies)
                {

                }
            }
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
