using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Exeggcute.src.gui;
using Exeggcute.src.entities;
using Exeggcute.src.contexts;

namespace Exeggcute.src
{
    class LevelSummaryMenu : IContext
    {
        protected int scoreGained;
        protected Difficulty difficulty;
        protected FloatTimer timer = new FloatTimer(1);
        protected const int DURATION = 600;
        protected Texture2D testTexture;
        public bool IsDone { get; protected set; }
        public HUD Hud { get; protected set; }
        public Player Player { get; protected set; }
        protected SpriteFont font;
        protected string nextName;
        public LevelSummaryMenu(Level level)
        {
            this.Hud = level.Hud;
            this.Player = Level.player;
            this.nextName = (int.Parse(level.Name) + 1).ToString();
        }

        public void Update(ControlManager controls)
        {
            float speed = 1.0f;
            if (controls[Ctrl.Action].IsPressed)
            {
                speed *= 2;
            }

            timer.Increment(speed);

            if (timer.Value >= DURATION &&
                controls[Ctrl.Action].DoEatPress())
            {
                IsDone = true;
                World.LoadNextLevel(Hud, Player, nextName, true);
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            Hud.Draw(batch, Player);
            batch.Draw(testTexture, new Vector2(100, 100), Color.White);
            string progress = timer.Value.ToString();
            batch.DrawString(font, progress, new Vector2(500, 500), Color.White);
            batch.End();
        }

        public void Load(ContentManager content)
        {

        }
        public void Unload()
        {

        }
        public void Dispose()
        {

        }
    }
}
