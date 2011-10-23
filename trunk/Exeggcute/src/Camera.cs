using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.input;

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

        public Camera(float distance, float angle, float speed)
        {
            Distance = distance;
            Position = new Vector3(0, 0, Distance);
            Target = new Vector3(0, 0, 0);
            Up = Vector3.Up;
            Near = 1.0f;
            Far = 10000.0f;
            FieldOfView = MathHelper.PiOver4;
            
        }

        float t1;
        float t2;
        float t3;

        /// <summary>
        /// Function purely for debugging purposes to move the camera around.
        /// </summary>
        public void Update(ControlManager controls)
        {
            if (controls[Ctrl.LShoulder].IsPressed)
            {
                t1 += 0.1f;
            }
            else if (controls[Ctrl.RShoulder].IsPressed)
            {
                t1 -= 0.1f;
            }

            if (controls[Ctrl.Left].IsPressed)
            {
                t2 += 0.1f;
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                t2 -= 0.1f;
            }

            if (controls[Ctrl.Up].IsPressed)
            {
                t3 += 0.1f;
            }
            else if (controls[Ctrl.Down].IsPressed)
            {
                t3 -= 0.1f;
            }
        }

        public Matrix GetView()
        {
            return 
                Matrix.CreateRotationZ(t1)
                * Matrix.CreateRotationY(t2)
                * Matrix.CreateRotationX(t3)
                * Matrix.CreateLookAt(Position, Target, Up)
                ;
        }

        public Matrix GetPlayerView()
        {
            return Matrix.CreateRotationY(MathHelper.Pi)
                * Matrix.CreateRotationZ(MathHelper.PiOver2)
                * Matrix.CreateLookAt(Position, Target, Up);
        }

        public Matrix GetProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView(FieldOfView, Engine.AspectRatio, Near, Far);
        }
    }
}
