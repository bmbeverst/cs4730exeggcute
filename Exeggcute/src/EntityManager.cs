using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    /// <summary>
    /// Checks whether dynamic entities are in contact with each other.
    /// This is not the class to process physics, only for 
    /// </summary>
    class EntityManager
    {
        public RepeatedSound collectSound = Assets.MakeRepeated("gulp", 5);
        public bool CollidePlayer(Player player, IEnumerable<Enemy> enemies)
        {
            if (!player.CanControl) return false;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Hitbox.Intersects(player.ModelHitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HitPlayer(IEnumerable<Shot> enemyShots, Player player)
        {
            if (player.IsInvulnerable) return false;
            foreach (Shot shot in enemyShots)
            {
                if (shot.Hitbox.Intersects(player.Hitbox))
                {
                    return true;
                }
                else if (!shot.HasGrazed && shot.Hitbox.Intersects(player.Hitbox))
                {
                    player.Graze(shot);
                    shot.Graze(player);
                }
            }
            return false;
        }

        public void CollideItems(IEnumerable<Item> itemList, Player player)
        {
            foreach (Item item in itemList)
            {
                if (item.Hitbox.Intersects(player.ModelHitbox))
                {
                    // this will call player.Collect. 
                    // do not call it here (double dispatch!)
                    collectSound.Play();
                    item.Collect(player);
                }
            }
        }

        public void Collide(Player player, IEnumerable<Shot> playerShots, IEnumerable<Enemy> enemies)
        {
            foreach (Shot shot in playerShots)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.Hitbox.Intersects(shot.Hitbox))
                    {
                        enemy.Collide(shot);
                        shot.Collide(enemy);
                        player.GivePoints(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all entities in worldspace that are outside the given 
        /// rectangle.
        /// </summary>
        public void FilterOffscreen<TEntity>(HashList<TEntity> entities, Rectangle rect, Dictionary<int, Entity3D> allEntities)
            where TEntity : Entity3D
        {
            List<TEntity> toRemove = new List<TEntity>();
            foreach (TEntity entity in entities)
            {
                if (!entity.ContainedIn(rect))
                {
                    allEntities.Remove(entity.ID);
                    toRemove.Add(entity);
                }
            }
            toRemove.ForEach(e => entities.Remove(e));
        }
        public void FilterDead<TEntity>(HashList<TEntity> entities, Dictionary<int, Entity3D> allEntities) 
            where TEntity : Entity
        {
            List<TEntity> removed = new List<TEntity>();
            foreach (TEntity entity in entities)
            {
                if (entity.IsTrash)
                {
                    allEntities.Remove(entity.ID);
                    removed.Add(entity);
                }
            }
            removed.ForEach(entity => entities.Remove(entity));
        }

        public void CollideTerrain<TEntity>(WangMesh terrain, TEntity entity, Rectangle gameArea) where TEntity : Entity3D
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

        public void CollideBoss(IEnumerable<Shot> playerShots, Boss boss)
        {
            BoundingSphere bossBox = boss.Hitbox;
            foreach (Shot shot in playerShots)
            {
                if (shot.Hitbox.Intersects(bossBox))
                {
                    shot.Collide(boss);
                    boss.Collide(shot);
                }
            }
        }

        public void CollideDying(WangMesh terrain)
        {
            foreach (Enemy enemy in Worlds.World.GetDying())
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



        internal void EatShots(IEnumerable<Shot> enemyShots, Rectangle rectangle)
        {
            foreach (Shot shot in enemyShots)
            {
                if (rectangle.Contains((int)shot.X, (int)shot.Y))
                {
                    shot.QueueDelete();
                }
            }
        }

        private void updateEntityLists<TEntity>(params IEnumerable<TEntity>[] entityLists)
            where TEntity : Entity3D
        {
            foreach (IEnumerable<TEntity> entities in entityLists)
            {
                foreach (TEntity entity in entities)
                {
                    entity.Update();
                }
            }
        }

        public void UpdateAll(Rectangle liveArea)
        {
            World world = Worlds.World;
            world.FilterOffscreen(this, liveArea);
            world.FilterDead(this);
            updateEntityLists<Entity3D>(world.GetGibList(),
                                        world.GetEnemies(),
                                        world.GetItemList(),
                                        world.GetEnemyShots(),
                                        world.GetPlayerShots()); 
        }



        public void DrawAll3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            World world = Worlds.World;
            drawEntityLists3D<Entity3D>(graphics, view, projection, world.GetGibList(),
                                                                  world.GetEnemies(),
                                                                  world.GetItemList(),
                                                                  world.GetEnemyShots(),
                                                                  world.GetPlayerShots());
        }

        private void drawEntityLists2D<TEntity>(SpriteBatch batch, params IEnumerable<TEntity>[] entityLists)
            where TEntity : Entity3D
        {
            foreach (IEnumerable<TEntity> entities in entityLists)
            {
                foreach (TEntity entity in entities)
                {
                    entity.Draw2D(batch);
                }
            }
        }

        private void drawEntityLists3D<TEntity>(GraphicsDevice graphics, Matrix view, Matrix projection, params IEnumerable<TEntity>[] entityLists)
            where TEntity : Entity3D
        {
            foreach (IEnumerable<TEntity> entities in entityLists)
            {
                foreach (TEntity entity in entities)
                {
                    entity.Draw3D(graphics, view, projection);
                }
            }
        }
    }
}
