using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class GrowBox
    {
        public Rectangle Rect { get; protected set; }
        public float Radius { get; protected set; }
        public float Speed { get; protected set; }

        public GrowBox(float speed)
        {
            this.Rect = new Rectangle(0, 0, 0, 0);
            this.Radius = 0;
            this.Speed = speed;
        }

        public void Update()
        {
            float prev = Radius;
            Radius += Speed;
            int diff = (int)(Radius - prev);
            Rect = new Rectangle(Rect.X - diff, Rect.Y - diff, Rect.Width + 2 * diff, Rect.Height + 2 * diff);
        }

        public void Reset()
        {
            Radius = 0;
            Rect = new Rectangle(0, 0, 0, 0);
        }


    }
}
