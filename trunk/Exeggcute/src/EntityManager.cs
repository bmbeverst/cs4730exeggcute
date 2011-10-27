using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;

namespace Exeggcute.src
{
    /// <summary>
    /// Checks whether dynamic entities are in contact with each other.
    /// This is not the class to process physics, only for 
    /// </summary>
    class EntityManager
    {
        public bool Collide(Player player, HashList<Enemy> enemies)
        {
            if (!player.CanControl) return false;
            var keys = enemies.GetKeys();
            foreach (Enemy enemy in keys)
            {
                if (enemy.OuterHitbox.Intersects(player.InnerHitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HitPlayer(HashList<Shot> enemyShots, Player player)
        {
            if (player.IsInvulnerable) return false;
            foreach (Shot shot in enemyShots.GetKeys())
            {
                if (shot.OuterHitbox.Intersects(player.InnerHitbox))
                {
                    return false;// true;
                }
                else if (!shot.HasGrazed && shot.OuterHitbox.Intersects(player.OuterHitbox))
                {
                    player.Graze(shot);
                    shot.Graze(player);
                }
            }
            return false;
        }

        public void Collide(HashList<Item> itemList, Player player)
        {
            foreach (Item item in itemList.GetKeys())
            {
                if (item.OuterHitbox.Intersects(player.OuterHitbox))
                {
                    // this will call player.Collect. 
                    // do not call it here (double dispatch!)
                    item.Collect(player);
                }
            }
        }

        public void Collide(HashList<Shot> playerShots, HashList<Enemy> enemies)
        {
            foreach (Shot shot in playerShots.GetKeys())
            {
                foreach (Enemy enemy in enemies.GetKeys())
                {
                    if (enemy.OuterHitbox.Intersects(shot.OuterHitbox))
                    {
                        enemy.Collide(shot);
                        shot.Collide(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all entities in worldspace that are outside the given 
        /// rectangle.
        /// </summary>
        public void FilterOffscreen<TEntity>(HashList<TEntity> entities, Rectangle rect)
            where TEntity : PlanarEntity3D
        {
            List<TEntity> toRemove = new List<TEntity>();
            foreach (TEntity entity in entities.GetKeys())
            {
                if (!entity.ContainedIn(rect))
                {
                    toRemove.Add(entity);
                }
            }
            toRemove.ForEach(e => entities.Remove(e));
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

        /// <summary>
        /// For handling enemy-enemy collisions
        /// </summary>
        /// <param name="entities"></param>
        public void Collide(List<Entity3D> entities)
        {
            for (int i = 0; i < entities.Count; i += 1)
            {
                for (int j = i + 1; j < entities.Count; j += 1)
                {
                    if (entities[i].OuterHitbox.Intersects(entities[j].OuterHitbox))
                    {

                    }
                }
            }
        }

        public void CollideTerrain<TEntity>(WangMesh terrain, TEntity entity, Rectangle gameArea) where TEntity : PlanarEntity3D
        {
            if (Math.Abs(entity.Position.Z - (terrain.Depth)) < 2)
            {
                float x = entity.X;
                float y = entity.Y;
                float z = terrain.Depth;
                if (entity.ContainedIn(gameArea))
                {
                    terrain.Impact(x, y, 0, 0);
                }
                entity.QueueDelete();
            }
        }

        public void CollideBoss(HashList<Shot> playerShots, Boss boss)
        {
            BoundingSphere bossBox = boss.OuterHitbox;
            foreach (Shot shot in playerShots.GetKeys())
            {
                if (shot.OuterHitbox.Intersects(bossBox))
                {
                    shot.Collide(boss);
                    boss.Collide(shot);
                }
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
                    enemy.Kill();
                }
            }
        }





        internal void EatShots(HashList<Shot> enemyShots, Rectangle rectangle)
        {
            foreach (Shot shot in enemyShots.GetKeys())
            {
                if (rectangle.Contains((int)shot.X, (int)shot.Y))
                {
                    shot.QueueDelete();
                }
            }
        }
    }
}
