using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.gui
{
    class GameOverMenu : Menu
    {
        protected HUD hud;
        protected Player player;
        Timer timer = new Timer(160);
        public GameOverMenu()
            : base(new List<Button>(), new Rectangle(0, 0, 0, 0), null, false)
        {

        }

        public void Attach(HUD hud, Player player, IContext parent)
        {
            this.hud = hud;
            this.player = player;
            this.Parent = parent;
            hud.DoFade(FadeType.Out);
        }
        public override void Update(ControlManager controls)
        {
            if (timer.IncrUntilDone())
            {
                throw new ResetException(null);
            }
            Parent.Update(controls);
        }
        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Parent.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            string gameover = "Game over!";
            hud.Draw(batch, player);
            batch.DrawString(font, gameover, new Vector2(490, 400), Color.White);
        }
    }
}
