using Exeggcute.src.entities;
using Exeggcute.src.scripting.arsenal;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.roster
{
    class RosterEntry
    {
        public Model Surface { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public ItemBatch HeldItems { get; protected set; }
        private Arsenal arsenal;

        public RosterEntry(Model model,
                           Texture2D texture,
                           BehaviorScript behavior, 
                           ItemBatch items,
                           Arsenal arsenal)
        {
            this.Surface = model;
            this.Texture = texture;
            this.Behavior = behavior;
            this.HeldItems = items;
            this.arsenal = arsenal;
        }

        public Arsenal GetArsenal(HashList<Shot> shotHandle)
        {
            return arsenal.Copy(shotHandle);
        }
    }
}
