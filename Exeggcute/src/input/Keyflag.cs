using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.input
{
    /// <summary>
    /// A wrapper for a 'byte' which indicated how many consecutive frames
    /// a ctrl has been active for.
    /// </summary>
    class Keyflag
    {
        public byte Value { get; protected set; }


        public void Update(bool inc)
        {
            if (inc) Incr();
            else Reset();
        }

        /// <summary>
        /// Value == 1
        /// </summary>
        public bool JustPressed
        {
            get { return Value == 1; } 
        }

        /// <summary>
        /// Value >= 1
        /// </summary>
        public bool IsPressed
        {
            get { return Value >= 1; }
        }

        /// <summary>
        /// if Value == 1 then 
        ///     begin Value += 1 ; true end 
        /// else false
        /// </summary>
        public bool DoEatPress()
        {
            if (Value == 1)
            {
                Value += 1;
                return true;
            }
            return false;
        }


        public void Incr()
        {
            Value = Math.Min((byte)(Value + 1), byte.MaxValue);
        }

        public void Reset()
        {
            Value = 0;
        }
    }
}
