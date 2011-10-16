using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src
{
    /// <summary>
    /// Checks whether dynamic entities are in contact with each other.
    /// This is not the class to process physics, only for 
    /// </summary>
    class CollisionManager
    {
        public void Update()
        {

        }

        public bool Collide(Player player, HashList<Enemy> enemies)
        {
            if (!player.CanControl) return false;
            var keys = enemies.GetKeys();
            foreach (Enemy enemy in keys)
            {
                if (enemy.Hitbox.Intersects(player.Hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Collide(HashList<Shot> enemyShots, Player player)
        {
            if (player.IsInvulnerable) return false;
            foreach (Shot shot in enemyShots.GetKeys())
            {
                if (shot.Hitbox.Intersects(player.Hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public void Collide(HashList<Shot> playerShots, HashList<Enemy> enemies)
        {
            List<Shot> shotsRemoved = new List<Shot>();
            List<Enemy> enemiesRemoved = new List<Enemy>();
            foreach (Shot shot in playerShots.GetKeys())
            {
                foreach (Enemy enemy in enemies.GetKeys())
                {
                    if (enemy.Hitbox.Intersects(shot.Hitbox))
                    {
                        enemy.Collide(shot);
                        shot.Collide(enemy);
                        if (shot.IsDestroyed)
                        {
                            shotsRemoved.Add(shot);
                        }
                        if (enemy.IsDying || enemy.IsDone)
                        {
                            enemiesRemoved.Add(enemy);
                        }
                    }
                }
            }
            shotsRemoved.ForEach(shot => playerShots.Remove(shot));
            enemiesRemoved.ForEach(enemy => enemies.Remove(enemy));
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
