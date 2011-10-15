using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.contexts;

namespace Exeggcute.src.gui
{
    class ButtonRow : Button
    {
        protected int cursorX;
        protected List<Button> row;

        public ButtonRow(List<Button> row, ContextEvent select, ContextEvent deselect)
            : base(select, deselect, null, null)
        {
            this.row = row;
        }

        public override void Update(ControlManager controls)
        {
            resolveCursor();
            row[cursorX].Update(controls);
            base.Update(controls);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            throw new NotImplementedException();
        }
        
        protected void resolveCursor()
        {
            Util.Clamp(cursorX, 0, row.Count - 1);
        }
        
        protected override void moveRight()
        {
            cursorX += 1;
        }
        
        protected override void moveLeft()
        {
            cursorX -= 1;
        }

        protected override void moveDown()
        {
            throw new NotImplementedException();
        }

        protected override void moveUp()
        {
            throw new NotImplementedException();
        }
    }
}
