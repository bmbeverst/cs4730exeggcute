using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.gui;

namespace Exeggcute.src.text
{
    class Conversation : IContext
    {
        public TextBoxList Box { get; protected set; }

        public bool IsDone { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Conversation(TextBoxList box)
        {
            this.Box = box;

            float x = Engine.Center2D.X - Box.Size.X/2;
            Rectangle gameRect = HUD.CalculateRect(Engine.XRes, Engine.YRes);
            float buffer = (gameRect.Width - Box.Size.X) / 2;
            float y = gameRect.Y + buffer;
            this.Position = new Vector2(x, Engine.YRes - y - Box.Size.Y);
        }

        public void Update(ControlManager controls)
        {
            Box.Update(controls);
            World.UpdateParent(controls);
            if (Box.IsDone)
            {
                IsDone = true;
                World.Pop(/*this*/);
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            World.DrawParent(graphics, batch);
            batch.Begin();

            Box.Draw(batch, Position, Color.White);
            batch.End();
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
