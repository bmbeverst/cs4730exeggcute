using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    /// <summary>
    /// General purpose timer class which wraps maximum/current values,
    /// removing the need for repetitive max/current fields wherever a 
    /// counter is needed.
    /// </summary>
    class Timer
    {
        public int Value { get; protected set; }
        public int Max { get; protected set; }
        public bool IsDone { get { return Value >= Max; } }
        public Timer(int max)
        {
            Max = max;
            Value = 0;
        }

        public Timer(int max, int initial)
        {
            Max = max;
            Value = initial;
        }

        public Timer(int max, bool done)
        {
            Max = max;
            if (done) Value = max;
        }

        public void Increment()
        {
            Value += 1;
        }

        public void Reset()
        {
            Value = 0;
        }

        public void ChangeMax(int newMax)
        {
            Max = newMax;
        }
    }
}
