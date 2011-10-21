using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    /// <summary>
    /// A wrapper for a section of a Texture2D. Parent class for 
    /// AnimatedSprite etc.
    /// </summary>
    class StaticSprite : Sprite
    {
        
        public StaticSprite(Texture2D texture, Point texelPos, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;
            FrameRect = new Rectangle(texelPos.X, texelPos.Y, width, height);
        }

        public StaticSprite(StaticAnimation anim)
        {
            Texture = anim.Texture;
            Width = anim.Width;
            Height = anim.Height;
            FrameRect = anim.GetFrameRect();
        }

        public override void Update()
        {
            //do nothing
        }

        /// <summary>
        /// Draws the sprite at the given position. 
        /// </summary>
        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            batch.Draw(Texture, pos, FrameRect, Color.White);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians)
        {
            batch.Draw(Texture, pos, FrameRect, Color.White, rotationRadians, TexelCenter, Vector2.One, SpriteEffects.None, 0);
        
        }

    }
}
