using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.arsenal
{
    class Arsenal
    {
        List<Spawner> spawners = new List<Spawner>();
        public List<OptionInfo> entries;
        public HashList<Shot> shotListHandle;
        public static Arsenal None = new Arsenal(new List<OptionInfo>(), new HashList<Shot>());
        public Arsenal(List<OptionInfo> entries, HashList<Shot> shotListHandle)
        {
            this.entries = entries;
            this.shotListHandle = shotListHandle;
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
                                           entry.ShotSound,
                                           shotListHandle);
                spawners.Add(next);
            }
        }

        public Arsenal Copy(Alignment alignment)
        {
            return new Arsenal(entries, GetShotHandle(alignment));
        }

        public Arsenal Copy(HashList<Shot> newShotHandle)
        {
            return new Arsenal(entries, newShotHandle);
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

        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection, Vector3 parentPos)
        {
            foreach (Spawner sp in spawners)
            {
                sp.Draw(graphics, view, projection);
            }

        }

        public void AttachShotHandle(HashList<Shot> shotListHandle)
        {
            this.shotListHandle = shotListHandle;
        }

        public static Arsenal Parse(string data)
        {
            string[] optionNames = data.Split(',');
            List<OptionInfo> options = new List<OptionInfo>();
            string alignmentString = optionNames[0];
            Alignment alignment = Util.ParseEnum<Alignment>(alignmentString);
            for (int i = 1; i < optionNames.Length; i += 1)
            {
                string name = optionNames[i];
                OptionInfo info = new OptionInfo(name);
                options.Add(info);
            }



            return new Arsenal(options, GetShotHandle(alignment));


        }

        public static HashList<Shot> GetShotHandle(Alignment alignment)
        {
            HashList<Shot> shotHandle;
            if (alignment == Alignment.Enemy)
            {
                shotHandle = World.EnemyShots;
            }
            else if (alignment == Alignment.Player)
            {
                shotHandle = World.PlayerShots;
            }
            else
            {
                throw new ExeggcuteError("Unexpected alignment type \"{0}\"", alignment);
            }
            return shotHandle;
        }
    }
}
