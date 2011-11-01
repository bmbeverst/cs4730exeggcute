using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities.items
{
    abstract class Item : ScriptedEntity
    {
        protected const float terminalSpeed = -0.3f;
        public Item(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, BehaviorScript behavior)
            : base(model, texture, scale, radius, rotation, behavior)
        {
            
        }

        public abstract Item Clone();
        public abstract void Collect(Player player);

        public override void Update()
        {
            if (Speed < terminalSpeed)
            {
                Speed = terminalSpeed;
            }
            base.Update();
        }


    }
}
