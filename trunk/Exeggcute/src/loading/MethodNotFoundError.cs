using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.loading
{
    class MethodNotFoundError : ExeggcuteError
    {
        public MethodNotFoundError(string methodName, string className)
            : base(getErrorMessage(methodName, className))
        {

        }
        protected static string getErrorMessage(string methodName, string className)
        {
            string message = "\"{0}\" in {1}";
            return string.Format(message, methodName, className);
        }

    }
}
