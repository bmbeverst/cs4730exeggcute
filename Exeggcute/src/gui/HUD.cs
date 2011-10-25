using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src.gui
{
    enum FadeType
    {
        None,
        In,
        Out,
    }
    class HUD
    {
        public const float WIDTH_RATIO = 0.8000000f;
        public const float HEIGHT_RATIO = 0.9333333f;

        public Rectangle GameRect { get; protected set; }

        private SpriteFont scoreFont;

        private Doodad leftBox;
        private Doodad rightBox;
        private Doodad bottomBox;
        private Doodad topBox;
        private Doodad fadeBox;
        private FadeType fadeType = FadeType.None;
        RectSprite fader;

        public HUD()
        {
            Resize(Engine.XRes, Engine.YRes);
            scoreFont = FontBank.Get("consolas");
        }

        public static Rectangle CalculateRect(int xres, int yres)
        {
            int gameHeight = (int)(HEIGHT_RATIO * yres);
            int gameWidth = (int)(gameHeight * WIDTH_RATIO);
            int boxWidth = (xres - gameWidth) / 2;
            int stripHeight = (yres - gameHeight) / 2;
            Rectangle result = new Rectangle(boxWidth, stripHeight, gameWidth, gameHeight);
            return result;
        }

        public void Resize(int xres, int yres)
        {
            GameRect = CalculateRect(xres, yres);
            fader = new RectSprite(GameRect.Width + 1, GameRect.Height + 1, Color.Black, true, true);
            fadeBox = new Doodad(fader, new Vector2(GameRect.X, GameRect.Y));
            int boxWidth = GameRect.X;
            int stripHeight = GameRect.Y;
            int gameWidth = GameRect.Width + 1;
            int gameHeight = GameRect.Height + 1;
            RectSprite sideSprite = new RectSprite(boxWidth, yres, Color.Black, true);
            RectSprite topSprite = new RectSprite(gameWidth, stripHeight, Color.Black, true);
            topBox = new Doodad(topSprite, new Vector2(boxWidth, 0));
            bottomBox = new Doodad(topSprite, new Vector2(boxWidth, yres - stripHeight));
            leftBox = new Doodad(sideSprite, new Vector2(0, 0));
            rightBox = new Doodad(sideSprite, new Vector2(boxWidth + gameWidth, 0));
        }

        float speed = 2f;
        public void Update()
        {
            if (fadeType != FadeType.None)
            {
                bool fadeIn = (fadeType == FadeType.In);
                fader.Fade(fadeIn, speed);
                if (fader.FadeDone)
                {
                    fadeType = FadeType.None;
                    fader.Reset();
                }
            }
        }

        /// <summary>
        /// Returns true if a fade is no longer taking place.
        /// </summary>
        public bool IsFading()
        {
            return fadeType != FadeType.None;
        }

        public void DoFade(FadeType type)
        {
            fadeType = type;
        }

        public void Draw(SpriteBatch batch, Player player)
        {
            //just messing around. draw whatever here
            leftBox.Draw(batch);
            rightBox.Draw(batch);
            topBox.Draw(batch);
            bottomBox.Draw(batch);
            fadeBox.Draw(batch);
            player.DrawHUD(batch, scoreFont);
        }
    }
}
