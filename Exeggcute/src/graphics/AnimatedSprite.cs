using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    /// <summary>
    /// Container for Texture2D and a list of animations. The animation tells
    /// us where on the Texture2D the current frame to be drawn is located.
    /// </summary>
    class AnimatedSprite : Sprite
    {
        //FIXME use caching
        public override int Width
        {
            get { return GetFrameRect().Width; }
        }
        //FIXME use caching
        public override int Height
        {
            get { return GetFrameRect().Height; }
        }
        public IAnimation CurrentAnimation 
        {
            get { return Animations[AnimationPtr]; }
        }

        public override Texture2D Texture 
        {
            //FIXME can be optimized, save the texture and only update if necessary
            get { return CurrentAnimation.Texture; }
            protected set { throw new Exception("don't do this"); }
        }

        public List<IAnimation> Animations { get; protected set; }
        public Rectangle GetFrameRect()
        {
            return CurrentAnimation.GetFrameRect();
        }
        public int AnimationPtr { get; protected set; }

        public AnimatedSprite(List<IAnimation> anims)
        {
            Animations = anims;
        }

        public static new Sprite LoadFromFile(string filename)
        {
            return Loaders.Sprite.LoadFromFile(filename);
        }

        public void SetAnimation(int n)
        {
            AnimationPtr = n;
        }

        public override void Update()
        {
            Animations[AnimationPtr].Update();
        }

        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            batch.Draw(Texture, pos, GetFrameRect(), Color.White);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians)
        {
            batch.Draw(Texture, pos, FrameRect, Color.White, rotationRadians, TexelCenter, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
