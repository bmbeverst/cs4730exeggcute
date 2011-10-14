using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src
{
    class HUD
    {
        public const float GAME_AREA_RATIO = 0.8000000f;

        public Rectangle GameRect { get; protected set; }

        private SpriteFont scoreFont;

        private Doodad leftBox;
        private Doodad rightBox;
        private Doodad bottomBox;
        private Doodad topBox;

        private float HEIGHT_RATIO = 0.9333333f;

        public HUD()
        {
            GameRect = Resize(Engine.XRes, Engine.YRes);
            scoreFont = FontBank.Get(FontName.font0);
        }

        public Rectangle Resize(int xres, int yres)
        {
            int gameHeight  = (int)(HEIGHT_RATIO * yres);
            int gameWidth   = (int)(gameHeight * GAME_AREA_RATIO);
            int boxWidth    = (xres - gameWidth)  / 2;
            int stripHeight = (yres - gameHeight) / 2;
            RectSprite sideSprite = new RectSprite(boxWidth, yres, Color.Black, true);
            RectSprite topSprite = new RectSprite(gameWidth, stripHeight, Color.Black, true);
            topBox = new Doodad(topSprite, new Vector2(boxWidth, 0));
            bottomBox = new Doodad(topSprite, new Vector2(boxWidth, yres - stripHeight));
            leftBox = new Doodad(sideSprite, new Vector2(0, 0));
            rightBox = new Doodad(sideSprite, new Vector2(boxWidth + gameWidth, 0));
            return new Rectangle(boxWidth, stripHeight, gameWidth, gameHeight);
        }


        public void Draw(SpriteBatch batch, Player player)
        {
            //just messing around. draw whatever here
            leftBox.Draw(batch);
            rightBox.Draw(batch);
            topBox.Draw(batch);
            bottomBox.Draw(batch);
            player.DrawHUD(batch, scoreFont);
        }
    }
}
