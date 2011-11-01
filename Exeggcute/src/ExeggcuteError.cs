using System;

namespace Exeggcute.src
{
    /// <summary>
    /// General unrecoverable programmer error.
    /// </summary>
    class ExeggcuteError : Exception
    {
        public ExeggcuteError(string message, params object[] args)
            : base(string.Format(message, args))
        {

        }

        public ExeggcuteError()
            : base("Unhandled programmer error")
        {

        }
    }
}
