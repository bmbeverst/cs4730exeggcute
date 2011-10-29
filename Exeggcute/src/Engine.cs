using System;
using System.Collections.Generic;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Exeggcute.src.entities;
using Exeggcute.src.assets;
using Exeggcute.src.text;
using Exeggcute.src.contexts;
using Exeggcute.src.sound;

namespace Exeggcute.src
{
    /// <summary>
    /// The engine of the game. This is one level removed from Game to 
    /// increase portability.
    /// </summary>
    class Engine
    {
        public static int XRes { get; protected set; }
        public static int YRes { get; protected set; }
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
        public static readonly Vector3 Jail = new Vector3(0, 0, -10000);

        static Engine()
        {
            XRes = 1200;
            YRes = 900;
            FPS = 60;
        }
        public static readonly float FIELD_OF_VIEW = MathHelper.PiOver4;
        public ControlManager controls;

        public static ScoreSet scoreSet;

        public Engine(GraphicsDevice graphics, ContentManager content, InputManager input)
        {
            World.Initialize(this, content, graphics);

            loadXNAContent(content);

            SpriteBank.LoadAll(content);
            BodyBank.LoadAll();

            ScriptBank.LoadAll();

            ItemBank.LoadAll();
            ItemBatchBank.LoadAll();

            ConversationBank.LoadAll();

            PlayerBank.LoadAll();
            loadMenus();

            scoreSet = new ScoreSet();
            controls = new ControlManager(input);
            TextBox.LoadSprites();
            //World.LoadTerrain();
            AssetManager.Commit();

        }

        private void loadXNAContent(ContentManager content)
        {
            
            TextureBank.LoadAll(content);//must be first

            FontBank.LoadAll(content);
            EffectBank.LoadAll(content);
            ModelBank.LoadAll(content);
            SfxBank.LoadAll(content);
            SongBank.LoadAll(content);
            mySound = SfxBank.MakeRepeated("1pew");
        }

        private void loadMenus()
        {
            World.Begin();
        }
        RepeatedSound mySound;
        int frame = 0;
        public void Update()
        {
            SfxBank.UpdateAll();
            //mySound.Play();
            frame += 1;
            controls.Update();
            World.Update(controls);
            if (controls[Ctrl.Quit].IsPressed)
            {
                Exit();
            }
            if (controls[Ctrl.Start].DoEatPress())
            {
                World.Pause();
            }
            
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            graphics.Clear(ClearOptions.DepthBuffer, Color.HotPink, 1.0f, 0);//easy to notice bleeding!
            World.Draw(graphics, batch);
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
            Environment.Exit(1);
        }
    }
}
