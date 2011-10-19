using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting.action;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class NewArsenal
    {

        protected static Dictionary<ArsenalEntry, Spawner> cache =
               new Dictionary<ArsenalEntry, Spawner>();

        List<Spawner> spawners = new List<Spawner>();

        public NewArsenal(List<ArsenalEntry> entries, HashList<Shot> shotListHandle)
        {
            foreach (ArsenalEntry entry in entries)
            {
                if (cache.ContainsKey(entry) && false)
                {
                    Spawner next = cache[entry];
                    next.AttachShotHandle(shotListHandle);
                    spawners.Add(cache[entry]);
                }
                else
                {
                    Spawner next = new Spawner(entry, shotListHandle);
                    cache[entry] = next;
                    spawners.Add(next);
                }
            }
        }

        public void Update(Vector3 parentPos, float parentAngle)
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Update(parentPos, parentAngle);
            }
        }

        public void Process(ShootAction shoot)
        {
            if (shoot.Switch)
            {
                spawners[shoot.ID].SpawnFor(shoot.Duration);
            }
            else
            {
                spawners[shoot.ID].Stop();
            }
        }

        public void StopAll()
        {
            spawners.ForEach(sp => sp.Stop());
        }
    }
}
