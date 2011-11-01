using Exeggcute.src.contexts;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    abstract class Button
    {
        protected Menu parent;
        protected ContextEvent onSelect;
        protected ContextEvent onDeselect;
        protected ContextEvent onActivate;
        protected ContextEvent onDeactivate;
        public abstract int Height { get; }
        //protected Drawable image;
        public bool IsActive { get; protected set; }
        public bool IsSelected { get; protected set; }
        public Button(ContextEvent select,
                      ContextEvent deselect,
                      ContextEvent activate,
                      ContextEvent deactivate)
        {
            this.onSelect = select;
            this.onDeselect = deselect;
            this.onActivate = activate;
            this.onDeactivate = deactivate;
            this.IsActive = false;
            this.IsSelected = false;
        }


        public void AttachParent(Menu parent)
        {
            this.parent = parent;
        }

        public void AttachOnActivate(ContextEvent activate)
        {
            this.onActivate = activate;
        }

        // A button has no position. That is determined by the menu
        public abstract void Draw(SpriteBatch batch, Vector2 pos);
        
        public virtual void Update(ControlManager controls)
        {
            
            if (controls[Ctrl.Up].JustPressed)
            {
                moveUp();
            }
            else if (controls[Ctrl.Down].JustPressed)
            {
                moveDown();
            }


            if (controls[Ctrl.Left].JustPressed)
            {
                moveLeft();
            }
            else if (controls[Ctrl.Right].JustPressed)
            {
                moveRight();
            }

            


        }
        
        protected abstract void moveUp();
        protected abstract void moveDown();
        protected abstract void moveLeft();
        protected abstract void moveRight();
    }
}
