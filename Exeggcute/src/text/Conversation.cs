using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.console.commands;
using Exeggcute.src.gui;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.text
{
    class Conversation : ConsoleContext
    {
        public TextBoxList BoxList { get; protected set; }

        public bool IsDone { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Conversation(SpriteFont font, string text, float rate)
        {
            this.BoxList = new TextBoxList(font, text, rate);
            init();
        }

        public Conversation(TextBoxList box)
        {
            this.BoxList = box;
            init();
        }

        private void init()
        {
            float x = Engine.Center2D.X - BoxList.Size.X / 2;
            Rectangle gameRect = HUD.CalculateRect(Engine.XRes, Engine.YRes);
            float buffer = (gameRect.Width - BoxList.Size.X) / 2;
            float y = gameRect.Y + buffer;
            this.Position = new Vector2(x, Engine.YRes - y - BoxList.Size.Y);
        }

        public void AttachParent(IContext parent)
        {
            this.Parent = parent;
        }

        public override void Update(ControlManager controls)
        {
            BoxList.Update(controls);
            ((Level)Parent).Update(controls, false);
            if (BoxList.IsDone)
            {
                IsDone = true;
                BoxList.Reset();
                World.Pop(/*this*/);
            }
        }

        public override void Draw2D(SpriteBatch batch)
        {
            Parent.Draw2D(batch);
            BoxList.Draw(batch, Position, Color.White);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Parent.Draw3D(graphics, camera);
        }

        public void Reset()
        {
            BoxList.Reset();
            Parent = null;
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
            return Assets.Conversation[s];
        }

        public static Conversation LoadFromFile(string filename)
        {
            return Loaders.Conversation.Load(filename);
        }
    }
}
