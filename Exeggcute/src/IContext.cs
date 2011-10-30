using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.console.commands;

namespace Exeggcute.src
{
    /// <summary>
    /// A Context is a generalized game area where input needs to be handled in a 
    /// certain way such as a level, a menu, or a pop-up.
    /// </summary>
    interface IContext
    {
        /// <summary>
        /// A handle for a context's parent which may well be null. This 
        /// allows a context to allow input to be passed up and draw etc
        /// </summary>
        IContext Parent { get; }

        /// <summary>
        /// Called once per frame when this context is on the top of the world
        /// stack, and additionally whenever this context's children decide 
        /// that it should. This method processes all non-drawing logic and 
        /// handles  input relevant for the context.
        /// </summary>
        void Update(ControlManager controls);

        /// <summary>
        /// Called once per frame when this context is on the top of the world
        /// stack, and additionally whenever this context's children decide 
        /// that it should. This method does all the drawing necessary for the 
        /// context.
        /// </summary>
        void Draw3D(GraphicsDevice graphics, Camera camera);

        /// <summary>
        /// Draws the 2D spritebatch elements of this context. The caller
        /// can be assured that batch.Begin and batch.End are called as
        /// desired.
        /// </summary>
        /// <param name="batch"></param>
        void Draw2D(SpriteBatch batch);



        /// <summary>
        /// Called whenever the context is popped from the stack.
        /// </summary>
        void Unload();

        /// <summary>
        /// Called as a way of "finalizing" the context in order to
        /// perform functions such as saving a new high score before the
        /// game is exited.
        /// </summary>
        void Dispose();

        
    }
}
