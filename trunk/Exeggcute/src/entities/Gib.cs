using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{

    class Gib : PlanarEntity3D
    {
        protected const float GIBSPEED = 1;
        protected const float speedFactor = 0.1f;
        protected const float OFFSET = 2;
        static Random rng = new Random();

        public Gib(Model model, Texture2D texture, float scale, float radius, Vector3 rotation)
            : base(model, texture, scale, radius, rotation, Engine.Jail)
        {

        }

        public Gib(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, Vector2 parentPos, float parentSpeed, float parentAngle)
            : base(model, texture, scale, radius, rotation, getOffset(parentPos))
        {
            float angle = rng.NextRadian();
            float speed = GIBSPEED;
            Vector2 parentVelocity = new Vector2(parentSpeed * FastTrig.Cos(parentAngle), 
                                                 parentSpeed * FastTrig.Sin(parentAngle));
            Vector2 myVelocity     = new Vector2(speed * FastTrig.Cos(angle), 
                                                 speed * FastTrig.Sin(angle));

            Vector2 sumVelocity    = myVelocity + parentVelocity;

            Angle = FastTrig.Atan2(sumVelocity.Y, sumVelocity.X);
            Speed = sumVelocity.Length()*speedFactor;
            VelocityZ = ((GIBSPEED/2 +  rng.Next() * GIBSPEED))*speedFactor;
            Mass = 10;
            
        }

        public Gib Copy()
        {
            return new Gib(Surface, Texture, Scale, Radius, ModelRotation);
        }


        private static Vector3 getOffset(Vector2 parentPos)
        {
            float xOffset = OFFSET * rng.NextSign() * rng.Next();
            float yOffset = OFFSET * rng.NextSign() * rng.Next();
            return new Vector3(parentPos.X + xOffset, parentPos.Y + yOffset, 0);
        }


    }
}
