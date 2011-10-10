using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class Camera
    {
        public Vector3 Position { get; protected set; }
        public Vector3 Target { get; protected set; }
        public Vector3 Up { get; protected set; }
        public float Distance { get; protected set; }
        public float Near { get; protected set; }
        public float Far { get; protected set; }
        public float FieldOfView { get; protected set; }

        public float ScrollAngle { get; protected set; }
        public float ScrollSpeed { get; protected set; }

        public Camera(float distance, float angle, float speed)
        {
            Distance = distance;
            Position = new Vector3(0, 0, Distance);
            Target = new Vector3(0, 0, 0);
            Up = Vector3.Up;
            Near = 1.0f;
            Far = 1000.0f;
            FieldOfView = MathHelper.PiOver4;

            ScrollSpeed = speed;
            ScrollAngle = angle;
            
        }

        public void Update()
        {

        }

        public Matrix GetView()
        {
            return Matrix.CreateLookAt(Position, Target, Up);
        }

        public Matrix GetProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView(FieldOfView, Engine.AspectRatio, Near, Far);
        }
    }
}
