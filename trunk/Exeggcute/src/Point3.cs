using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;

namespace Exeggcute.src
{
    class Point3
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Z { get; protected set; }

        public Point3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Point3 Parse(string s)
        {
            try
            {
                string cleaned = s.Replace("(", "").Replace(")", "").Replace(" ", "");
                string[] nums = cleaned.Split(',');
                int x = int.Parse(nums[0]);
                int y = int.Parse(nums[1]);
                int z = int.Parse(nums[2]);
                return new Point3(x, y, z);
            }
            catch
            {
                throw new ParseError("Unable to convert {0} to Point3", s);
            }
        }
    }
}
