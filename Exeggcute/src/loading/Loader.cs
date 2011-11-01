using System.Text.RegularExpressions;

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
}
