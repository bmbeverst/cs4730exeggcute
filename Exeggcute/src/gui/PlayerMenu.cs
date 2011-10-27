using System;
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
        public static List<Button> MakeButtons(SpriteFont font, Color fontColor, bool canUseCustom)
        {
            if (!PlayerBank.IsLoaded)
            {
                PlayerBank.LoadAll();
            }
            SpriteFont myfont = FontBank.Get("consolas");
            Color color = Color.Black;
            Rectangle bounds = new Rectangle(500, 500, 100, 100);
            List<Button> buttons = new List<Button>();
            foreach (Player player in PlayerBank.GetAll())
            {
                Console.WriteLine("yes");
                if (!canUseCustom && player.IsCustom)
                {
                    continue;
                }

                buttons.Add(new PlayerButton(player, myfont, color));
                
            }

            if (buttons.Count <= 0)
            {
                throw new ParseError("No player found which can be used in this context");
            }
            return buttons;
        }

        public bool CanUseCustom { get; protected set; }

        public PlayerMenu(List<Button> buttons, Rectangle bounds, bool canUseCustom)
            : base(buttons, bounds, false)
        {
            this.CanUseCustom = canUseCustom;
        }


        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            base.Draw(graphics, batch);
            batch.End();
        }

        public override void Back()
        {
            World.Back();
        }
    }
}
