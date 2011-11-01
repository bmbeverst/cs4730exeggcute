using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    /// <summary>
    /// A Doodad is simply an IDrawable2D along with a position
    /// at which to draw it.
    /// </summary>
    class Doodad
    {
        public IDrawable2D Image;
        public Vector2 Position;

        public Doodad(IDrawable2D image, Vector2 pos)
        {
            Image = image;
            Position = pos;
        }

        public Doodad(Rectangle rect, Color color, bool filled)
        {
            Image = new RectSprite(rect.Width, rect.Height, color, filled);
            Position = new Vector2(rect.X, rect.Y);
        }

        public void Draw(SpriteBatch batch)
        {
            Image.Draw(batch, Position);
        }
    }
}
