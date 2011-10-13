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

        public bool Collide(Player player, List<CommandEntity> enemies)
        {
            if (!player.CanControl) return false;
            foreach (Entity3D enemy in enemies)
            {
                if (enemy.Hitbox.Intersects(player.Hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public void Collide(List<Shot> playerShots, List<CommandEntity> enemies)
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
