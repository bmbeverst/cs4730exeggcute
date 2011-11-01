using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    abstract class Entity3D : Entity
    {
        public virtual Vector3 Position { get; protected set; }
        
        public virtual BoundingSphere OuterHitbox { get; protected set; }

        public float Radius { get; protected set; }
        

        public Entity3D(Vector3 pos, float radius)
        {
            this.Position = pos;
            this.Radius = radius;
            this.OuterHitbox = new BoundingSphere(Position, radius);
        }

        public Entity3D(Vector3 pos)
        {
            this.Position = pos;
            this.Radius = 0;
            this.OuterHitbox = new BoundingSphere(Position, 0);
        }


        public void SetPosition(Vector3 newpos)
        {
            Position = newpos;
        }

        public virtual void Update()
        {
            OuterHitbox = new BoundingSphere(Position, OuterHitbox.Radius);
        }

        public abstract void Draw(GraphicsDevice graphics, Matrix view, Matrix projection);

    }
}
