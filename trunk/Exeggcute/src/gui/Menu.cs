using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.gui
{
    abstract class Menu : IContext
    {
        protected int cursor;
        protected List<Button> buttons;
        protected bool loops;
        
        public Menu(List<Button> buttons, bool loops)
        {
            this.cursor = 0;
            this.buttons = buttons;
            this.loops = loops;
        }
        public virtual void Update(ControlManager controls)
        {
            resolveCursor();
            foreach (Button button in buttons)
            {
                button.Update(controls);
            }
            //update the selected button specially
            
        }

        public virtual void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            foreach (Button button in buttons)
            {
                Console.WriteLine("URFF");
                button.Draw(batch, new Vector2(50, 50));
                // draw according to a layout NOT according to button position.
                // A button has no position
            }
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

        public virtual void Move(Direction dir)
        {

        }

        protected virtual void resolveCursor()
        {
            if (loops) cursor = (cursor + buttons.Count) % buttons.Count;
            else cursor = Util.Clamp(cursor, 0, buttons.Count - 1);
        }

        public virtual void Cleanup()
        {

        }


    }
}
