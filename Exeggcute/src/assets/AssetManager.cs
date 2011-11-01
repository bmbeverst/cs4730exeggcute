using System;
using System.Collections.Generic;

namespace Exeggcute.src.assets
{
    static class AssetManager
    {
        private static List<string> log = new List<string>();
        private static bool seenFailure;
        public static void LogFailure(string message, params object[] args)
        {
            seenFailure = true;
            message = string.Format(message, args);
            Console.Error.WriteLine(message);
            log.Add(message);
        }

        public static void Commit()
        {
            if (seenFailure)
            {
                Console.Error.WriteLine("You program has died for the following reason(s):");
                foreach (string msg in log)
                {
                    Console.Error.WriteLine(msg);
                }
                Util.Die("");
            }
            
        }
    }
}
