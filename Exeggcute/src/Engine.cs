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
            YRes = 600;
        }

        public ControlManager controls;
        private static List<Menu> menus = new List<Menu>();
        public ContextStack world = new ContextStack();
        private Player3D player;
        public Engine(GraphicsDevice device, ContentManager content, InputManager input)
        {
            loadScripts(content);
            loadTextures(content);
            loadFonts(content);
            loadEffects(content);
            loadModels(content);
            loadSprites(content);
            loadMenus();
            controls = new ControlManager(input);
            player = new Player3D(ModelName.testcube);
            //
            //hardcoded
            world.PushContext(new Level(device, content));
            
        }


        public static Menu GetMenu(MenuID id)
        {
            return menus[(int)id];
        }

        private void loadScripts(ContentManager content)
        {
            ScriptBank.LoadAll(content);
        }

        private void loadTextures(ContentManager content)
        {
            TextureBank.LoadAll(content);
        }

        private void loadFonts(ContentManager content)
        {
            FontBank.LoadAll(content);
        }

        private void loadEffects(ContentManager content)
        {
            EffectBank.LoadAll(content);
        }

        private void loadModels(ContentManager content)
        {
            ModelBank.LoadAll(content);
        }

        private void loadSprites(ContentManager content)
        {
            SpriteBank.LoadAll(content);
            //HACK
            LifeItem.HUDSprite = SpriteBank.Get(SpriteName.life);
            BombItem.HUDSprite = SpriteBank.Get(SpriteName.bomb);
        }

        private void loadMenus()
        {
            //Main menu not ready
            /*MainMenu main = new MainMenu(world);
            world.PushContext(main);*/
        }

        public void Update()
        {
            controls.Update();
            player.Update(controls);
            world.Update(controls);
            if (controls[Ctrl.Quit].IsPressed)
            {
                Exit();
            }
            
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();
            world.Draw(graphics, batch);
            batch.End();
        }

        public void Exit()
        {
            controls.WriteToFile();
            Environment.Exit(1);
        }
    }
}
