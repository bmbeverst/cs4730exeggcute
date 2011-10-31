﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.graphics;
using Exeggcute.src.entities;
using Exeggcute.src.scripting;

namespace Exeggcute.src.gui
{
    /// <summary>
    /// Player select menu
    /// </summary>
    class PlayerMenu : Menu
    {
        public static List<Button> MakeButtons(SpriteFont font, Color fontColor)
        {
            SpriteFont myfont = Assets.Font["consolas"];
            Color color = Color.White;
            List<Button> buttons = new List<Button>();
            foreach (Player player in Assets.Player.GetAssets())
            {
                buttons.Add(new PlayerButton(player, myfont, color));
            }

            if (buttons.Count <= 0)
            {
                throw new ParseError("No player found which can be used in this context");
            }
            return buttons;
        }

        List<Player> playerCopies = new List<Player>();

        SpriteText menuTitle;
        EntityManager manager = new EntityManager();

        public PlayerMenu(List<Player> players, List<Button> buttons, Rectangle bounds)
            : base(buttons, bounds, false)
        {

            menuTitle = new SpriteText(font, "Player select", Color.White);
            foreach (Player player in players)
            {

                player.DoDemo();
                playerCopies.Add(player);
            }

            
        }
        public override void Update(ControlManager controls)
        {
            manager.FilterOffscreen(World.PlayerShots, new Rectangle(-55, -40, 110, 80));
            base.Update(controls);
            playerCopies[cursor].Update();
            foreach (Shot shot in World.PlayerShots.GetKeys())
            {
                shot.Update();
            }
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {

            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            //playerCopies[cursor].SetPosition(new Vector3(0, 0, 0));
            playerCopies[cursor].Draw(graphics, view, projection);
            foreach (Shot shot in World.PlayerShots.GetKeys())
            {
                shot.Draw(graphics, view, projection);
            }
            base.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            base.Draw2D(batch);
            menuTitle.Draw(batch, new Vector2(50, 400));
        }

        public override void Back()
        {
            World.Back();
        }
    }
}
