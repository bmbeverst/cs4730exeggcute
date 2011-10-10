using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    class HUD
    {
        public const int GAME_AREA_WIDTH = 400;

        private Doodad leftBox;
        private Doodad rightBox;
        
        public HUD()
        {
            int boxWidth = (Engine.XRes - GAME_AREA_WIDTH) / 2;
            RectSprite left = new RectSprite(boxWidth, Engine.YRes, Color.Black, true);
            RectSprite right = new RectSprite(boxWidth, Engine.YRes, Color.Black, true);
            leftBox = new Doodad(left, new Vector2(0, 0));
            rightBox = new Doodad(right, new Vector2(boxWidth + GAME_AREA_WIDTH, 0));
        
        }

        public void Draw(SpriteBatch batch)
        {
            //just messing around. draw whatever here
            leftBox.Draw(batch);
            rightBox.Draw(batch);
        }
    }
}
