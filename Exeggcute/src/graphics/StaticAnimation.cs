using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    class StaticAnimation : IAnimation
    {
        public StaticAnimation(TextureName texName, Point frame)
        {
            Texture = TextureBank.Get(texName);
            CurrentFrame = frame;
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
