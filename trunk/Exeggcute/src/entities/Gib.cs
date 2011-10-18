using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class Gib : PlanarEntity3D
    {
        protected const float GIBSPEED = 2;
        protected const float OFFSET = 2;
        static Random rng = new Random();
        public Gib(ModelName modelName, Vector2 parentPos, float parentSpeed, float parentAngle)
            : base(modelName, getOffset(parentPos))
        {
            float angle = rng.NextRadian();
            float speed = GIBSPEED;
            Vector2 parentVelocity = new Vector2(parentSpeed * FastTrig.Cos(parentAngle), 
                                                 parentSpeed * FastTrig.Sin(parentAngle));
            Vector2 myVelocity     = new Vector2(speed * FastTrig.Cos(angle), 
                                                 speed * FastTrig.Sin(angle));

            Vector2 sumVelocity    = myVelocity + parentVelocity;

            Angle = FastTrig.Atan2(sumVelocity.Y, sumVelocity.X);
            Speed = sumVelocity.Length();
            VelocityZ = GIBSPEED/2 +  rng.Next() * GIBSPEED;
            Mass = 10;
            
        }

        private static Vector3 getOffset(Vector2 parentPos)
        {
            float xOffset = OFFSET * rng.NextSign() * rng.Next();
            float yOffset = OFFSET * rng.NextSign() * rng.Next();
            return new Vector3(parentPos.X + xOffset, parentPos.Y + yOffset, 0);
        }
    }
}
