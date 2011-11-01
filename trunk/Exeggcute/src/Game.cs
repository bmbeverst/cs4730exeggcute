using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Exeggcute.src.text;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.loading;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Exeggcute.src
{
    // http://gemini.cs.virginia.edu:8080/~team3/
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch batch;
        private Engine engine;
        public Point ScreenSize;
        public static Game GameHandleDONTUSE;
        public Game()
        {
            GameHandleDONTUSE = this;
            graphicsManager = new GraphicsDeviceManager(this);
            graphicsManager.PreferredBackBufferHeight = Engine.YRes;
            graphicsManager.PreferredBackBufferWidth = Engine.XRes;
            //graphicsManager.PreferMultiSampling = true;
            /*graphicsManager.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;*/
            //graphicsManager.IsFullScreen = true;
            InactiveSleepTime = TimeSpan.Zero;
            
            Window.Title = "Exeggcute";
        }


        protected override void LoadContent()
        {
            ScreenSize = new Point(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            batch = new SpriteBatch(GraphicsDevice);

            Reset(null);

            base.LoadContent();
        }

        /// <summary>
        /// Passing null to this function will keep the same dataset
        /// </summary>
        /// <param name="name"></param>
        public void Reset(string name)
        {

            Loaders.Reset();
            World.Reset();
            Assets.Reset();
            
            ContentManager newContent = new ContentManager(Content.ServiceProvider);
            Content = newContent;
            Content.RootDirectory = Engine.ContentRoot;

            engine = new Engine(GraphicsDevice, Content, new InputManager(), name);
            World.ConsoleWrite("Reloaded to dataset \"{0}\"", name);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                engine.Update();
            }
            catch (ResetException re)
            {
                Reset(null);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            engine.Draw(GraphicsDevice, batch);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Note:
        /// Initialize is called before Draw, so the length of time spent 
        /// executing code in this method will be experienced by the user 
        /// as a delay before he or she sees the initial game screen. 
        /// </summary>
        protected override void Initialize()
        {
            try
            {
                //ServerTest test = new ServerTest();
                //test.Test();
            }
            catch (Exception error)
            {
                Console.WriteLine("{0}\nFailed to connect to server", error.Message); 
            }
            base.Initialize();
        }

        public new void Exit()
        {
            engine.Exit();
        }
        
    }
}


