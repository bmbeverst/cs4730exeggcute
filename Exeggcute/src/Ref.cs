using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    struct Ref<T>
    {
        private T held;
        public Ref(T held)
        {
            this.held = held;
        }

        public static T operator~(Ref<T> refr)
        {
            return refr.held;
        }

        public Ref<T> Give()
        {
            return new Ref<T>(held);
        }
    }
}
