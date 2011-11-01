using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    /// <summary>
    /// Used for anything which has a concept of being drawn at a particular 
    /// point.
    /// Not suitable for things which hold their positions internally.
    /// </summary>
    interface IDrawable2D
    {
        int Height { get; }
        void Draw(SpriteBatch batch, Vector2 pos);
        void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians);
    }
}
