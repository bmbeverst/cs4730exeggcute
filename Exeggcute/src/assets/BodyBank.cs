using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.loading;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;

namespace Exeggcute.src.assets
{
    class BodyBank
    {
        protected static CustomBank<BodyInfo> bank =
            new CustomBank<BodyInfo>("data/bodies");

        public static BodyInfo Get(string name)
        {
            try
            {
                return bank[name];
            }
            catch
            {
                AssetManager.LogFailure("Unable to get {0}", name);
                return bank.GetAssets()[0];
            }
        }

        public static void LoadAll()
        {
            string failures = "";
            foreach (string file in bank.AllFiles)
            {
                try
                {
                    BodyInfo info = new BodyInfo(file);
                    bank.Put(info, file);
                }
                catch (ParseError error)
                {
                    failures += string.Format("Load failure in \"{0}\"\n", file);
                    failures += error.Message + "\n";
                }
            }
            if (failures.Length != 0)
            {
                AssetManager.LogFailure("Failed to load body:\n{0}", failures);
            }
        }
    }
}
