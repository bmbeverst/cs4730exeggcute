using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.gui;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.console.commands;

namespace Exeggcute.src.text
{
    class Conversation : ConsoleContext
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

        public void AttachParent(IContext parent)
        {
            this.Parent = parent;
        }

        public override void Update(ControlManager controls)
        {
            Box.Update(controls);
            ((Level)Parent).Update(controls, false);
            if (Box.IsDone)
            {
                IsDone = true;
                Box.Reset();
                World.Pop(/*this*/);
            }
        }

        public override void Draw2D(SpriteBatch batch)
        {
            Parent.Draw2D(batch);
            Box.Draw(batch, Position, Color.White);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Parent.Draw3D(graphics, camera);
        }

        public void Reset()
        {
            Box.Reset();
            IsDone = false;
        }

        public void Load(ContentManager content)
        {

        }

        public override void Unload()
        {

        }

        public override void Dispose()
        {

        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            throw new NotImplementedException();
        }

        public static Conversation Parse(string s)
        {
            return ConversationBank.Get(s);
        }
    }
}
