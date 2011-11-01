using Exeggcute.src.loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    abstract class Sprite : IDrawable2D
    {
        public virtual Texture2D Texture { get; protected set; }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }
        public virtual Vector2 TexelCenter
        {
            get { return new Vector2(Width / 2, Height / 2); }
        }
        public virtual Rectangle FrameRect { get; protected set; }

        public abstract void Update();
        public abstract void Draw(SpriteBatch batch, Vector2 pos);
        public abstract void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians);

        public static Sprite LoadFromFile(string filename)
        {
            return Loaders.Sprite.LoadFromFile(filename);
        }

    }
}
