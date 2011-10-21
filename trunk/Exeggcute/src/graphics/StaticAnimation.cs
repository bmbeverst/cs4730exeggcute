using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    class StaticAnimation : IAnimation
    {
        public StaticAnimation(string texName, Point frame, int width, int height)
        {
            Texture = TextureBank.Get(texName);
            CurrentFrame = frame;
            Width = width;
            Height = height;
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
