using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;

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
            
            foreach (Shot shot in playerShots.GetKeys())
            {
                foreach (Enemy enemy in enemies.GetKeys())
                {
                    if (enemy.Hitbox.Intersects(shot.Hitbox))
                    {
                        enemy.Collide(shot);
                        shot.Collide(enemy);
                    }
                }
            }
        }

        public void FilterDead<TEntity>(HashList<TEntity> entities) where TEntity : Entity
        {
            List<TEntity> removed = new List<TEntity>();
            foreach (TEntity entity in entities.GetKeys())
            {
                if (entity.IsTrash)
                {
                    removed.Add(entity);
                }
            }
            removed.ForEach(entity => entities.Remove(entity));
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

        public void CollideTerrain<TEntity>(WangMesh terrain, TEntity entity) where TEntity : PlanarEntity3D
        {
            if (Math.Abs(entity.Position.Z - terrain.Depth) < 2)
            {
                float x = entity.X;
                float y = entity.Y;
                float z = terrain.Depth;
                terrain.Impact(x, y, 0, 0);
                entity.QueueDelete();
            }
        }

        public void CollideDying(WangMesh terrain)
        {
            HashList<Enemy> enemies = World.DyingList;
            foreach (Enemy enemy in enemies.GetKeys())
            {
                if (Math.Abs(enemy.Position.Z - terrain.Depth) < 2)
                {
                    float x = enemy.X;
                    float y = enemy.Y;
                    float z = terrain.Depth;
                    terrain.Impact(x, y, 0, 0);
                    enemy.Die();
                }
            }
        }




    }
}
