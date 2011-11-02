using System;
using System.Collections.Generic;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.loading.specs;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.arsenal
{
    class Arsenal
    {
        List<Spawner> spawners = new List<Spawner>();
        public List<OptionInfo> entries;
        public static Arsenal None = new Arsenal(new List<OptionInfo>());
        public Arsenal(List<OptionInfo> entries)
        {
            this.entries = entries;
            foreach (OptionInfo entry in entries)
            {
                Spawner next = new Spawner(entry.Body.Model, 
                                           entry.Body.Texture, 
                                           entry.Body.Scale.Value, 
                                           entry.Body.Radius.Value,
                                           entry.Body.Rotation.Value,
                                           entry.Damage, 
                                           entry.Trajectory, 
                                           entry.Spawn, 
                                           entry.Behavior,
                                           entry.ShotSound);
                spawners.Add(next);
            }
        }

        public Arsenal Copy()
        {

            return new Arsenal(entries);
        }

        public void Update(Vector3 parentPos, float parentAngle)
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Update(parentPos, parentAngle);
            }
        }

        public void UpdateMovers(Vector3 parentPos, float parentAngle)
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.UpdateMover(parentPos, parentAngle);
            }
        }

        public void Fire(ShootAction shoot)
        {
            Console.WriteLine("FIRING {0}", shoot.ID);
            int id = (int)shoot.ID.Value;
            if (shoot.Duration != -1)
            {
                spawners[id].SpawnFor(shoot.Duration);
            }
            else
            {
                spawners[id].Stop();
            }
        }

        public void FireAll()
        {
            spawners.ForEach(sp => sp.Spawn());
        }

        public void StopAll()
        {
            spawners.ForEach(sp => sp.Stop());
        }

        public void Draw3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            foreach (Spawner sp in spawners)
            {
                sp.Draw3D(graphics, view, projection);
            }

        }

        public void CheckAlignment(Alignment alignment)
        {
            foreach (Spawner spawn in spawners)
            {
                spawn.SetAlignment(alignment);
            }
        }

        public static Arsenal Parse(string data)
        {
            string[] optionNames = data.Split(',');
            List<OptionInfo> options = new List<OptionInfo>();
            for (int i = 0; i < optionNames.Length; i += 1)
            {
                string name = optionNames[i];
                OptionInfo info = Assets.Option[name];
                options.Add(info);
            }



            return new Arsenal(options);
        }
    }
}
