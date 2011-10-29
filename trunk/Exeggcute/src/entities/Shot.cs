using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    class Shot : ScriptedEntity
    {

        public int Damage { get; protected set; }

        public bool HasGrazed { get; protected set; }
        public Shot(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, TrajectoryScript trajectory, int damage)
            : base(model, texture, scale, radius, rotation, trajectory)
        {
            this.Damage = damage;
        }

        public Shot Clone(Vector3 pos, float angle)
        {
            //FIXME cast
            Shot clone = new Shot(Surface, Texture, Scale, Radius, DegRotation, (TrajectoryScript)script, Damage);
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
