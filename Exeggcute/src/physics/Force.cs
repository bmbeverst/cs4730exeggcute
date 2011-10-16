using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.physics
{
    interface Force
    {
        Vector3 GetMagnitude(Vector3 pos, float mass);
        bool IsDestroyed();
    }
}
