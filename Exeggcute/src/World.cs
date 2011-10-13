﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.gui;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.input;
using Exeggcute.src.contexts;

namespace Exeggcute.src
{
    /// <summary>
    /// Stores and processes the current contexts in game.
    /// Example: The settings menu is pushed on top of the main menu,
    /// or the ReallyQuit? menu is pushed on top of the PauseMenu is 
    /// pushed on the Level Context.
    /// </summary>
    static class World 
    {
        private static Stack<IContext> stack = new Stack<IContext>();
        private static bool isInitialized = false;
        private static ContentManager content;
        private static GraphicsDevice graphics;

        public static void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            World.content = content;
            World.graphics = graphics;
            isInitialized = true;
        }


        public static void Update(ControlManager controls)
        {
            stack.Peek().Update(controls);
        }
        
        public static void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            stack.Peek().Draw(graphics, batch);
        }

        public static void LoadLevel()
        {
            stack.Push(new Level(graphics, content));
        }

        public static void Process(ContextEvent ent)
        {

        }

        public static void Process(LoadLevelEvent ent)
        {
            stack.Push(new Level(graphics, content));
        }

        public static void Process(QuitEvent ent)
        {
            //FIXME
            Environment.Exit(0);
        }
        
        // Do I need the events at all if im doing it this way?
        public static void SendMove(Direction dir)
        {
            ((Menu)(stack.Peek())).Move(dir);
        }
        
        public static void SendAccept()
        {
            
        }
        
        public static void SendBack()
        {
          
        }

        public static void Pause()
        {
            stack.Push(new PauseMenu());
        }

        public static void Unpause()
        {
            if (!(stack.Peek() is PauseMenu))
            {
                throw new InvalidOperationException("Only can unpause from the pause menu");
            }
            stack.Pop();
        }

        public static void PushContext(IContext context)
        {
            stack.Push(context);
        }
        
        /// <summary>
        /// Removes all elements from the stack, calling their Cleanup method
        /// as they are removed.
        /// </summary>
        public static void Cleanup(ContentManager content)
        {
            for (int i = 0; i < stack.Count; i += 1)
            {
                stack.Pop().Unload();
            }
        }



    }
}
