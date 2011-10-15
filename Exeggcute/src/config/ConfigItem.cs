using System;
using System.Collections.Generic;

namespace Exeggcute.src.config
{
    class ConfigItem
    {
        public const char SETTING_DELIM = ':';
        public const char SETTING_START = '[';
        public const char SETTING_END = ']';
        public string Content { get; protected set; }
        public Setting Type { get; protected set; }
        public string Value { get; protected set; }
        public int LineNo { get; protected set; }
        public List<string> PreComments { get; protected set; }
        public ConfigItem(string line, int num, List<string> comments)
        {
            PreComments = new List<string>(comments);
            Content = line;
            parseSetting();
            LineNo = num;
        }

        private void parseSetting()
        {
            string trimmed = Content.Trim(SETTING_START, SETTING_END);
            string[] tokens = trimmed.Split(SETTING_DELIM);
            if (tokens.Length != 2)
            {
                Util.Warn("Failed to split setting \"{0}\"", Content);
                return;
            }
            Type = settingFromString(tokens[0]);
            Value = tokens[1];
            return;
        }

        private static Setting settingFromString(string name)
        {
            Setting result;
            bool success = Enum.TryParse<Setting>(name, true, out result);
            if (!success)
            {
                Util.Warn("Failed to parse setting \"{0}\"", name);
                return Setting.INVALID;
            }
            return result;
        }

        private void getDefault()
        {

        }

        public override string ToString()
        {
            string result = "";
            foreach (string comment in PreComments)
            {
                result += comment + "\n";
            }
            result += Content + "\n";
            return result;
        }

        public static bool IsComment(string line)
        {

            return line.Length == 0 || line.Trim(Util.Whitespace)[0] != SETTING_START;
        }


    }
}
