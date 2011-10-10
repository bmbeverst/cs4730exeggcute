using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    class RectSprite : IDrawable2D
    {
        public Point Size { get; protected set; }
        public Texture2D DotTexture { get; protected set; }
        public Color FillColor { get; protected set; }
        public int Width
        {
            get { return Size.X; } 
        }
        public int Height
        {
            get { return Size.Y; } 
        }

        private Vector2[] scaleVectors;

        public RectSprite(int width, int height, Color color, bool filled)
        {
            Point size = new Point(width, height);
            init(size, color, filled);
        }

        private void init(Point size, Color color, bool filled)
        {
            DotTexture = TextureBank.Get(TextureName.dot);
            Size = size;
            FillColor = color;
            
            if (filled)
            {
                scaleVectors = new Vector2[] {
                    //Position------------------Scale
                    new Vector2(0, 0),          new Vector2(Width, Height)
                };
            }
            else
            {
                scaleVectors = new Vector2[] {
                    //Position------------------Scale
                    new Vector2(0,     0),      new Vector2(Width, 1),
                    new Vector2(0,     Height), new Vector2(Width, 1),
                    new Vector2(0,     0),      new Vector2(1,     Height),
                    new Vector2(Width, 0),      new Vector2(1,     Height)

                };
            }
        }

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            SpriteEffects eff = SpriteEffects.None;
            for (int i = 0; i < scaleVectors.Length; i += 2)
            {
                batch.Draw(DotTexture, pos + scaleVectors[i], null, FillColor, 0, Vector2.Zero, scaleVectors[i+1], eff, 0);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians)
        {
            throw new NotImplementedException("Not intended to be rotatable since this often represents a hitbox. A hitbox does not behave correctly when rotated out of the box.");
        }
    }
}
