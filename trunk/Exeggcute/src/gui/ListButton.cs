using Exeggcute.src.contexts;
using Exeggcute.src.graphics;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    /// <summary>
    /// A Button intended to be used in a Menu as a horizontally spaced
    /// list of buttons.
    /// </summary>
    class ListButton : Button 
    {

        public SpriteText Text { get; protected set; }

        public override int Height
        {
            get { return Text.Height; }
        }
        public ListButton(ContextEvent activate, SpriteText text)
            : base(null, null, activate, null)
        {
            this.Text = text;
        }

        

        public override void Update(ControlManager controls)
        {
            IsActive = true;
            if (controls[Ctrl.Action].DoEatPress())
            {
                
                parent.Select();
                onActivate.Process();
                
            }
            else if (controls[Ctrl.Cancel].DoEatPress())
            {
                
                parent.Back();
            }
            base.Update(controls);
        }
        static float scale = 2;
        Vector2[] offsets = new Vector2[] {
            new Vector2(scale,scale),
            //new Vector2(scale,-scale),
            //new Vector2(-scale,-scale),
            //new Vector2(-scale,scale),
             

        };
        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            if (Text == null) return;
            foreach (Vector2 offset in offsets)
            {
                Text.Draw(batch, pos + offset, Color.Black);
            }
            Text.Draw(batch, pos);
        }

        protected override void moveUp()
        {
            World.SendMove(Direction.Up);
            IsActive = false;
        }
        
        protected override void moveDown()
        {
            World.SendMove(Direction.Down);
            IsActive = false;
        }

        protected override void moveLeft()
        {
            //throw new NotImplementedException();
        }

        protected override void moveRight()
        {
            //throw new NotImplementedException();
        }
    }
}
