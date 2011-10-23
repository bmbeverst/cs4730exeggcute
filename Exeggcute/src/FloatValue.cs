using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

    }

    class Float3
    {
        public FloatValue X { get; protected set; }
        public FloatValue Y { get; protected set; }
        public FloatValue Z { get; protected set; }
        public Vector3 Vector
        {
            get { return new Vector3(X.Value, Y.Value, Z.Value); }
        }

        public Float3(FloatValue x, FloatValue y, FloatValue z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }



    }

    enum RangeType
    {
        Int,
        Float
    }


}
