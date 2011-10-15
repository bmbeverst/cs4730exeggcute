﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.input;
using Exeggcute.src.contexts;

namespace Exeggcute.src.gui
{
    abstract class Button
    {
        protected ContextEvent onSelect;
        protected ContextEvent onDeselect;
        protected ContextEvent onActivate;
        protected ContextEvent onDeactivate;
        //protected Drawable image;
        public bool IsActive { get; protected set; }
        public bool IsSelected { get; protected set; }
        public Button(ContextEvent select,
                      ContextEvent deselect,
                      ContextEvent activate,
                      ContextEvent deactivate)
        {
            onSelect = select;
            onDeselect = deselect;
            onActivate = activate;
            onDeactivate = deactivate;
            IsActive = false;
            IsSelected = false;
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