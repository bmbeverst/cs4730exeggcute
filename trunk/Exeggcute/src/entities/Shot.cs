using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.entities
{
    class Shot : ScriptedEntity
    {

        public int Damage { get; protected set; }
        protected ArsenalEntry arsenalParams;
        public bool HasGrazed { get; protected set; }
        public Shot(ArsenalEntry entry)
            : base(entry.Surface, entry.Trajectory)
        {
            Damage = 10;
            this.arsenalParams = entry;
        }

        public Shot Clone(Vector3 pos, float angle)
        {
            Shot clone = new Shot(arsenalParams);
            clone.Angle = angle;
            clone.Position = pos;
            return clone;
        }
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Called when the shot collides with a hostile entity.
        /// </summary>
        public virtual void Collide(ScriptedEntity entity)
        {
            IsTrash = true;
        }


        internal void Graze(Player player)
        {
            HasGrazed = true;
            //TODO add a sound here!
        }
    }
}
