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

namespace Exeggcute.src
{
    class NewArsenal
    {
        List<Spawner> spawners = new List<Spawner>();
        public List<ArsenalEntry> entries;
        public HashList<Shot> shotListHandle;
        public NewArsenal(List<ArsenalEntry> entries, HashList<Shot> shotListHandle)
        {
            this.entries = entries;
            this.shotListHandle = shotListHandle;
            foreach (ArsenalEntry entry in entries)
            {
                Spawner next = new Spawner(entry, shotListHandle);
                spawners.Add(next);
            }
        }

        public NewArsenal Copy()
        {
            return new NewArsenal(entries, shotListHandle);
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
            int id = (int)shoot.ID.Value;
            if (shoot.TurnOn)
            {
                spawners[id].SpawnFor(shoot.Duration);
            }
            else
            {
                spawners[id].Stop();
            }
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
    }
}
