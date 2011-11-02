using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;

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
            IsMouseVisible = true;
            
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
            Assets.Reset();
            
            ContentManager newContent = new ContentManager(Content.ServiceProvider);
            Content = newContent;
            Content.RootDirectory = Engine.ContentRoot;

            engine = new Engine(GraphicsDevice, Content, new InputManager(), name);
            if (name != null) Worlds.World.ConsoleWrite("Reloaded to dataset \"{0}\"", name);
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
                Reset(re.Name);
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
            base.Initialize();
        }

        public new void Exit()
        {
            engine.Exit();
        }
        
    }
}


