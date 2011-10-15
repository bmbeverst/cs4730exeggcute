using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    class ScoreMenu : Menu
    {
        protected ScoreSet scores;
        
        public ScoreMenu(ScoreSet scores)
            : base(getButtons(), false)
        {
            this.scores = scores;
        }

        private static void loadScores()
        {

        }

        private static List<Button> getButtons()
        {
            return null;
        }

        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            base.Draw(graphics, batch);

        }
        
    }


}
