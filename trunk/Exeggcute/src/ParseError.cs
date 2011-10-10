using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class ParseError : Exception
    {
        public ParseError(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}
