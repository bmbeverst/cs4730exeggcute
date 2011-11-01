using System;

namespace Exeggcute.src
{
    class DeletedResourceError : Exception
    {
        public DeletedResourceError(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}
