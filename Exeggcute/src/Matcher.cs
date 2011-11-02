using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Exeggcute.src
{
    class Matcher
    {
        private string regex;
        private RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
        public Matcher(string regex)
        {
            this.regex = regex;
        }

        public Matcher(string regex, RegexOptions options)
        {
            this.regex = regex;
            this.options = options;
        }

        public bool this[string input]
        {
            get { return Regex.IsMatch(input, regex, options); } 
        }
    }
}
