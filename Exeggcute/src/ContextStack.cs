﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.gui;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src
{
    /// <summary>
    /// Stores and processes the current contexts in game.
    /// Example: The settings menu is pushed on top of the main menu,
    /// or the ReallyQuit? menu is pushed on top of the PauseMenu is 
    /// pushed on the Level Context.
    /// </summary>
    class ContextStack 
    {
        private Stack<IContext> stack = new Stack<IContext>();
        private List<MenuEvent> eventList = new List<MenuEvent>();

        public ContextStack()
        {

        }

        public void Update(ControlManager controls)
        {
            stack.Peek().Update(controls);
            handleEvents();
        }
        
        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            stack.Peek().Draw(graphics, batch);
        }
        
        public void SendEvent(MenuEvent eve)
        {
            eventList.Add(eve);
        }
        
        // Do I need the events at all if im doing it this way?
        public void SendMove(Direction dir)
        {
            ((Menu)(stack.Peek())).Move(dir);
        }
        
        public void SendAccept()
        {
            
        }
        
        public void SendBack()
        {
          
        }

        public void PushContext(IContext context)
        {
            stack.Push(context);
        }
        
        private void handleEvents()
        {
            /*foreach (MenuEvent m_event in eventList)
            {
                MenuEventType type = m_event.Type;
                if (type == MenuEventType.Pop)
                {
                    stack.Pop().Cleanup();
                }
                else if (type == MenuEventType.Push)
                {
                    stack.Push(Engine.GetMenu(m_event.ID));
                }
                else if (type == MenuEventType.Move)
                {
                    stack.Peek().Move(m_event.Dir);
                }
                else if (type == MenuEventType.Replace)
                {
                    this.Cleanup();
                    stack.Push(Engine.GetMenu(m_event.ID));
                }
                else
                {
                    string error = string.Format("Unimplemented event type: {0}", type);
                    throw new ArgumentException(error);
                }
            }*/
            eventList.Clear();
        }
        /// <summary>
        /// Removes all elements from the stack, calling their Cleanup method
        /// as they are removed.
        /// </summary>
        public void Cleanup(ContentManager content)
        {
            for (int i = 0; i < stack.Count; i += 1)
            {
                stack.Pop().Unload();
            }
        }



    }
}
