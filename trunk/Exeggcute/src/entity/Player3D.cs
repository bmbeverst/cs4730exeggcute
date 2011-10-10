using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class Player3D : PlanarEntity3D
    {
        public Player3D(ModelName name, Vector3 pos)
            : base(name, pos)
        {

        }
        // Just messing around, nothing important in here
        public void Update(ControlManager controls)
        {
            float speed = 1;
            int dx = 0;
            int dy = 0;
            if (controls[Ctrl.Up].IsPressed)
            {
                dy = 1;
            }
            else if (controls[Ctrl.Down].IsPressed)
            {
                dy = -1;
            }

            if (controls[Ctrl.Left].IsPressed)
            {
                dx = -1;
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                dx = 1;
            }
            if ((dx | dy) == 0)
            {
                Speed = 0;
                Angle = 0;
            }
            else
            {
                Speed = speed;
                Angle = FastTrig.Atan2(dy, dx);
            }



            if (controls[Ctrl.RShoulder].IsPressed)
            {
                Z += speed;
            }
            else if (controls[Ctrl.LShoulder].IsPressed)
            {
                Z -= speed;
            }
            base.Update();
        }
    }
}
