using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Exeggcute.src.loading
{
    abstract class Loader
    {
        protected string currentField;
        protected bool matches(string regex)
        {
            string wholeRegex = string.Format("^{0}$", regex);
            return Regex.IsMatch(currentField, wholeRegex, RegexOptions.IgnoreCase);
        }
    }

    abstract class LoadedInfo : Loader
    {
        public virtual string[] Tokenize(string input)
        {
            return Util.RemoveSpace(input).Split(':');
        }
        public static void AssertInitialized(LoadedInfo self)
        {
            Type thisType = self.GetType();
            FieldInfo[] fields = thisType.GetFields();
            List<string> fieldsLeft = new List<string>();
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(self) == null)
                {
                    fieldsLeft.Add(field.Name);
                }
            }
            if (fieldsLeft.Count > 0)
            {
                string message = "The following fields were not initialized:\n";
                foreach (string field in fieldsLeft)
                {
                    message += string.Format("    {0}\n", field);
                }
                throw new ExeggcuteError(message);
            }

        }
    }

}
