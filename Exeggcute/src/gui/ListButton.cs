﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;

namespace Exeggcute.src.gui
{
    /// <summary>
    /// A Button intended to be used in a Menu as a horizontally spaced
    /// list of buttons.
    /// </summary>
    class ListButton : Button 
    {
        public IDrawable2D Image { get; protected set; }
        public ListButton(MenuEvent activate, IDrawable2D image)
            : base(null, null, activate, null)
        {
            Image = image;
        }
        
        public override void Update(ControlManager controls)
        {
            IsActive = true;
            if (controls[Ctrl.Action].DoEatPress())
            {
                World.SendEvent(onActivate);
            }
            else if (controls[Ctrl.Cancel].DoEatPress())
            {
                //ContextStack.Send(MenuEventType.Pop);
            }
            base.Update(controls);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            Image.Draw(batch, pos);
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
            throw new NotImplementedException();
        }

        protected override void moveRight()
        {
            throw new NotImplementedException();
        }
    }
}
