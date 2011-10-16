using System;
using System.Collections.Generic;

namespace Exeggcute.src.config
{
    enum Setting
    {
        RESOLUTION,
        SOUND,
        UP,
        INVALID
    }

    class Configuration
    {
        const string defaults =
@"Settings
The screen resolution. Valid values are:
800x600, 1024x768, 1280x960
[RESOLUTION:800x600]

The sound volume in game. Values are between 0 and 100.
[SOUND:100]

";
        List<ConfigItem> Lines = new List<ConfigItem>();
        public string Filepath { get; protected set; }
        public Dictionary<Setting, ConfigItem> settings = new Dictionary<Setting, ConfigItem>();
        public static Configuration DefaultConfig = new Configuration(defaults, true);

        
        
        public Configuration(string filepath)
        {
            Filepath = filepath;
            List<string> lines = Util.ReadLines(filepath);
            init(lines);
        }

        private Configuration(string fileContents, bool isPrivate)
        {
            /*Filepath = "defaults";
            List<string> lines = Util.SplitLines(fileContents);
            init(lines);*/
        }

        private void init(List<string> lines)
        {
            List<string> commentLines = new List<string>();
            for (int i = 0; i < lines.Count; i += 1 )
            {
                //Console.WriteLine(lines[i]);
                if (ConfigItem.IsComment(lines[i]))
                {
                    commentLines.Add(lines[i]);
                    continue;
                }
                //Console.WriteLine("Setting line: {0}", lines[i]);
                Lines.Add(new ConfigItem(lines[i], i, commentLines));
                commentLines.Clear();
            }

            foreach (ConfigItem optLine in Lines)
            {
                settings.Add(optLine.Type, optLine);
            }
        }

        public ConfigItem GetItem(Setting type)
        {
            return settings[type];
        }

        public void WriteFile()
        {
            string outtext = "";
            Setting[] settingValues = (Setting[])Enum.GetValues(typeof(Setting));
            for(int i = 0; i < settingValues.Length; i += 1)
            {
                Setting opt = settingValues[i];
                if (opt == Setting.INVALID) continue;

                if (settings.ContainsKey(opt))
                {
                    outtext += settings[opt].ToString();
                }
                else
                {
                    Util.Warn("Adding default setting which was missing: {0}", opt);
                    //outtext += DefaultConfig.GetItem(opt).ToString();
                }
            }
        }
    }
}
