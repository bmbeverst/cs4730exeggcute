using System.Collections.Generic;

namespace Exeggcute.src.loading
{
    class LoaderError : ExeggcuteError
    {
        public LoaderError()
            : base()
        {

        }
        public LoaderError(string message, params object[] args)
            : base(message, args)
        {

        }

        public LoaderError(List<string> errors)
            : base(mergeLines(errors))
        {
        }

        private static string mergeLines(List<string> errors)
        {
            string message = "";
            foreach (string error in errors)
            {
                message += error + "\n";
            }
            return message;
        }
    }
}
