using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src
{
    class HealthBar
    {
        protected RectSprite background;
        protected RectSprite foreground;
        protected int maxHealth;
        protected int currentHealth;
        protected float width;
        protected float height;
        protected Timer timer;
        protected SpriteFont font;
        public HealthBar(int maxHealth, float width, float height, Timer timer)
        {
            this.font = Assets.Font["consolas"];
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
            this.width = width;
            this.height = height;
            this.timer = timer;
            initRects();
        }

        private void initRects()
        {
            background = new RectSprite((int)width, (int)height, Color.Black, true);
            foreground = new RectSprite((int)width, (int)height, Color.Red, true);
        }

        public void Update(int hp)
        {
            this.currentHealth = hp;
        }
        float offset = 1;
        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            background.Draw(batch, pos + new Vector2(offset, offset));
            foreground.DrawSolidWidth(batch, pos, width*(currentHealth/(float)maxHealth), height);
            string timerString = string.Format("{0:000}/{1:000}", timer.Value, timer.Max);
            batch.DrawString(font, timerString, new Vector2(0, 0), Color.White);
        }
    }
}
