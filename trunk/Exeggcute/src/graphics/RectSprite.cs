using System;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private float alpha;

        public RectSprite(int width, int height, Color color, bool filled, bool blanked=false)
        {
            Point size = new Point(width, height);
            init(size, color, filled);
            if (blanked) alpha = 255;
        }

        public RectSprite(Rectangle rect, Color color, bool filled, bool blanked = false)
        {
            Point size = new Point(rect.Width, rect.Height);
            init(size, color, filled);
            if (blanked) alpha = 255;
        }

        private void init(Point size, Color color, bool filled)
        {
            //FIXME generate dot at initialization
            DotTexture = Assets.Texture["dot"];
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

        public bool FadeDone { get; protected set; }
        
        public void Fade(bool fadeIn, float speed)
        {
            if (fadeIn)
            {
                alpha -= speed;
                byte newAlpha = (byte)Math.Max(0, alpha);
                FillColor = new Color(FillColor.R, FillColor.G, FillColor.B, newAlpha);
                if (FillColor.A == 0)
                {
                    FadeDone = true;
                    alpha = 0;
                }
            }
            else
            {
                alpha += speed;
                byte newAlpha = (byte)Math.Min(255, alpha);
                FillColor = new Color(FillColor.R, FillColor.G, FillColor.B, newAlpha);
                if (FillColor.A == 255)
                {
                    FadeDone = true;
                    alpha = 255;
                }
            }
        }

        public void Reset()
        {
            FadeDone = false;
        }

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            SpriteEffects eff = SpriteEffects.None;
            for (int i = 0; i < scaleVectors.Length; i += 2)
            {
                batch.Draw(DotTexture, pos + scaleVectors[i], null, FillColor, 0, Vector2.Zero, scaleVectors[i+1], eff, 0);
            }
        }

        public void DrawSolidWidth(SpriteBatch batch, Vector2 pos, float w, float h)
        {
            batch.Draw(DotTexture, pos, null, FillColor, 0, Vector2.Zero, new Vector2(w, h), SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians)
        {
            throw new NotImplementedException("Not intended to be rotatable since this often represents a hitbox. A hitbox does not behave correctly when rotated out of the box.");
        }
    }
}
