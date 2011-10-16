using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting
{
    class ParseError : ExeggcuteError
    {
        
        public ParseError(Exception error, string line, string filepath)
            : base(String.Format("{0}\nFailed to parse line {1} in {2}", error.Message, line, filepath))
        {

        }

        public ParseError(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }

    }
}
