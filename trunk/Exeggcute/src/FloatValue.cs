using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace Exeggcute.src
{
    class FloatValue
    {
        private float val = 9999;
        public virtual float Value
        {
            get { return val; }
            set { val = value; }
        }
        public FloatValue(float val)
        {
            this.val = val;
        }

        public virtual FloatValue Mult(float rhs)
        {
            val *= rhs;
            return this;
        }

        public virtual FloatValue FromDegrees()
        {
            return Mult(FastTrig.degreesToRadians);
        }

        public static FloatValue Parse(string s)
        {
            if (s.Contains('|'))
            {
                return FloatRange.Parse(s);
            }
            float floatValue;
            if (Regex.IsMatch(s, "d"))
            {
                s = s.Replace("d", "");
                floatValue = float.Parse(s) * FastTrig.degreesToRadians;
            }
            else
            {
                floatValue = float.Parse(s);
            }

            return new FloatValue(floatValue);
        }

        protected FloatValue()
        {

        }

    }

    class FloatRange : FloatValue
    {
        protected static Random rng = new Random();

        public override float Value
        {
            get { return Min + range * rng.Next(); }
            set { Min = value; }
        }
        public float Min { get; protected set; }
        public float Max { get; protected set; }
        protected float range;

        public FloatRange(float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.range = Max - Min;
        }

        public override FloatValue Mult(float x)
        {
            Min *= x;
            Max *= x;
            range = Max - Min;
            return this;
        }

        public override FloatValue FromDegrees()
        {
            return Mult(FastTrig.degreesToRadians);
        }

        public new static FloatRange Parse(string s)
        {
            string flattened = Util.RemoveSpace(s).Replace("]", "").Replace("[", "");
            string[] split = flattened.Split('|');
            float min = float.Parse(split[0]);
            float max = float.Parse(split[1]);
            return new FloatRange(min, max);
        }

    }

    class Float3
    {
        public FloatValue X { get; protected set; }
        public FloatValue Y { get; protected set; }
        public FloatValue Z { get; protected set; }
        public Vector3 Vector3
        {
            get { return new Vector3(X.Value, Y.Value, Z.Value); }
        }
        public Vector2 Vector2
        {
            get { return new Vector2(X.Value, Y.Value); }
        }

        public Float3(FloatValue x, FloatValue y, FloatValue z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Float3(float x, float y, float z)
        {
            this.X = new FloatValue(x);
            this.Y = new FloatValue(y);
            this.Z = new FloatValue(z);
        }

        public static Float3 Parse(string s)
        {
            s = Util.RemoveSpace(s);
            s = s.Replace("(", "").Replace(")", "");
            string[] ranges = s.Split(',');
            FloatValue x = FloatValue.Parse(ranges[0]);
            FloatValue y = FloatValue.Parse(ranges[1]);
            FloatValue z = FloatValue.Parse(ranges[2]);
            return new Float3(x, y, z);
        }



    }

    enum RangeType
    {
        Int,
        Float
    }


}
