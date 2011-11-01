using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class ResetException : ExeggcuteError
    {
        public string Name { get; protected set; }

        public ResetException(string name)
        {
            this.Name = name;
        }
    }
}
