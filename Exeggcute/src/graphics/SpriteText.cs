using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.graphics
{
    class SpriteText : IDrawable2D
    {
        public SpriteFont Font { get; protected set; }
        public string Text { get; protected set; }
        public Color FillColor { get; protected set; }
        public Vector2 Center { get; protected set; }
        public int Height
        {
            get { return (int)Font.MeasureString(Text).Y; }
        }

        public SpriteText(SpriteFont font, string text, Color color)
        {
            this.Text = text;
            this.Font = font;
            this.FillColor = color;
            this.Center = Font.MeasureString(Text) / 2;
        }

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            batch.DrawString(Font, Text, pos, Color.White);
        }

        public void Draw(SpriteBatch batch, Vector2 pos, float rotationRadians)
        {
            batch.DrawString(Font, Text, pos, FillColor, rotationRadians, Center, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
