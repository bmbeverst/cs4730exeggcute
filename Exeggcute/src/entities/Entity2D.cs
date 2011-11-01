using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    class Entity2D : Entity
    {

        public virtual Sprite Sprite { get; protected set; }
        public virtual Vector2 Position { get; protected set; }
        public virtual float X 
        { 
            get { return Position.X; } 
            set { Position = new Vector2(value, Position.Y); } 
        }
        public virtual float Y
        {
            get { return Position.Y; }
            set { Position = new Vector2(Position.X, value); }
        }

        public Entity2D(Texture2D texture, Vector2 pos)
        {
            Sprite = new StaticSprite(texture, new Point(0, 0), 16, 16);
            Position = pos;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, Position);
        }


    }
}
