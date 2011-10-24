using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class FloatTimer
    {
        public float Value { get; protected set; }
        public float PrevValue { get; protected set; }
        public float Rate { get; protected set; }

        public FloatTimer(float rate)
        {
            Rate = rate;
            Value = 0;
        }

        public void Increment()
        {
            PrevValue = Value;
            Value += Rate;
        }

        public void Increment(float factor)
        {
            PrevValue = Value;
            Value += Rate*factor;
        }

        public int GetDelta()
        {
            return (int)Value - (int)PrevValue;
        }
    }
}
