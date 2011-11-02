using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.gui;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    class LevelSummaryMenu : ConsoleContext
    {
        protected bool validScore;
        protected int scoreGained;
        protected Difficulty difficulty;
        protected FloatTimer timer = new FloatTimer(1);
        protected const int DURATION = 50;
        public bool IsDone { get; protected set; }
        public HUD Hud { get; protected set; }
        public Player Player { get; protected set; }
        protected string nextName;
        protected SpriteFont font;

        public LevelSummaryMenu(Level level, Player player, HUD hud)
        {
            this.Parent = level;
            this.font = Assets.Font["consolas"];
            this.validScore = level.ValidScore;
            this.Hud = hud;
            this.Player = player;
            this.nextName = (int.Parse(level.Name) + 1).ToString();
            this.difficulty = level.Difficulty;
            this.scoreGained = Player.Score - level.initialScore;

        }

        public override void Update(ControlManager controls)
        {
            timer.Increment(1.0f);

            if (timer.Value >= DURATION &&
                controls[Ctrl.Action].DoEatPress())
            {
                IsDone = true;
                Worlds.World.LoadNextLevel(Hud, Player, true);
            }
        }

        public override void Draw2D(SpriteBatch batch)
        {
            Hud.Draw(batch, Player);
            string timeLeft = timer.Value >= DURATION ? "Press shoot to continue!" : "Loading...";
            string scoreString = string.Format("Points gained: {0:000,000,000}", scoreGained);

            batch.DrawString(font, scoreString, new Vector2(500, 500), Color.White);
            batch.DrawString(font, timeLeft, new Vector2(500, 550), Color.White);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            
        }

        public override void Unload()
        {

        }

        public override void Dispose()
        {

        }

    }
}
