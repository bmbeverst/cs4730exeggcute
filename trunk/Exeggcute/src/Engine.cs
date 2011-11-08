using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.input;
using Exeggcute.src.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Exeggcute.src.scripting.task;
using Exeggcute.src.entities;
using Exeggcute.src.config;

namespace Exeggcute.src
{
    /// <summary>
    /// The engine of the game. This is one level removed from Game to 
    /// increase portability.
    /// </summary>
    class Engine
    {
        public static int XRes
        {
            get { return Settings.Global.Video.XRes; }
        }

        public static int YRes 
        { 
            get { return Settings.Global.Video.YRes; }
        }
        public static int FPS { get; protected set; }
        public static Vector2 Center2D
        {
            get { return new Vector2(XRes / 2, YRes / 2); }
        }
        public static float AspectRatio
        {
            get { return (float)XRes / (float)YRes; }
        }

        /// <summary>
        /// FIXME: HACK
        /// Sort of hackish. Entities are placed here when they spawn so that
        /// they don't show up. A better alternative would be to give things
        /// a bool which specifies that they are "Active" or "Initialized"
        /// or something
        /// </summary>
        public static readonly Vector3 Jail = new Vector3(-10000, -10000, -10000);

        static Engine()
        {
            FPS = 60;
        }

        public static readonly bool WRITE_LOG = false;
        public static readonly float FIELD_OF_VIEW = MathHelper.PiOver4;
        public ControlManager controls;

        public static ScoreSet scoreSet;
        public static string ContentRoot = "ExeggcuteContent";
        public static string DataRoot = "data";

        protected Manifest manifest;
        protected DataSet dataSet;

        public Engine(GraphicsDevice graphics, ContentManager content, InputManager input, string dataSetName)
        {
            Settings.Reset();
            Worlds.Reset(this, content, graphics);
            Assets.LoadAll(content);

            Worlds.World.MakeOverlay();

            manifest = new Manifest(dataSetName);
            dataSet = new DataSet(manifest.DataFileName, manifest.ForceOverwrite);

            Worlds.World.Begin();
            
            scoreSet = new ScoreSet();
            controls = new ControlManager(input);

            TextBox.LoadSprites();
            
            AssetManager.Commit();

            Worlds.World.RunInit();
        }

        
        int frame = 0;
        public void Update()
        {
            
            Assets.UpdateSfx();
            //mySound.Play();
            frame += 1;
            controls.Update();


            if (controls[Ctrl.Quit].IsPressed)
            {
                Exit();
            }

            Worlds.World.Update(controls);
            //after this point if the console is in front, 
            // no key presses may be eaten
            
            if (controls[Ctrl.Start].DoEatPress())
            {
                Worlds.World.Pause();
            }
            
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            graphics.Clear(ClearOptions.DepthBuffer, Color.HotPink, 1.0f, 0);//easy to notice bleeding!
            Worlds.World.Draw(graphics, batch);
            resetRenderState(graphics);
        }

        protected void resetRenderState(GraphicsDevice graphics)
        {
            graphics.BlendState = BlendState.AlphaBlend;
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public void Exit()
        {
            controls.WriteToFile();
            scoreSet.WriteLocal();
            Util.WriteLog();
            Worlds.World.Unload();
            Environment.Exit(1);
        }
    }
}
