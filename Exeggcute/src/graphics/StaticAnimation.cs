using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    class StaticAnimation : IAnimation
    {
        public StaticAnimation(Texture2D texture, Point frame, int width, int height)
        {
            this.Texture = texture;
            this.CurrentFrame = frame;
            this.Width = width;
            this.Height = height;
        }

        public override void Update()
        {
            //intentionally blank
        }

        public override void ChangeSpeed(int newSpeed)
        {
            //intentionally blank
        }


        
    }
}
