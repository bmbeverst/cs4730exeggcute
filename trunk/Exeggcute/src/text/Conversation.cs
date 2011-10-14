using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.text
{
    class Conversation : IContext
    {
        public IContext Parent { get; protected set; }
        public TextBoxList Box { get; protected set; }

        public Conversation(IContext parent, TextBoxList box)
        {
            Parent = parent;
            Box = box;
        }

        public void Update(ControlManager controls)
        {
            Box.Update(controls);
            Parent.Update(controls);
            if (Box.IsDone)
            {
                World.Pop(this);
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            Parent.Draw(graphics, batch);
            Box.Draw(batch, Vector2.Zero, Color.White);
        }

        public void Load(ContentManager content)
        {

        }

        public void Unload()
        {

        }

        public void Dispose()
        {

        }
    }
}
