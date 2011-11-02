using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Exeggcute.src.scripting;

namespace Exeggcute.src.config
{
    class GlobalConfigs
    {
        private static readonly string FILENAME = "config";

        
        public ControlConfig Control = new ControlConfig();
        public AudioConfig Audio = new AudioConfig();
        public VideoConfig Video = new VideoConfig();

        public void WriteDefaults()
        {
            string output = "#Settings";

            output += "\n@Audio\n";
            foreach (var pair in Audio.GetDefault())
            {
                output += Util.MakeFieldPair(pair.Key, pair.Value) + '\n';
            }

            output += "\n@Video\n";
            foreach (var pair in Video.GetDefault())
            {
                output += Util.MakeFieldPair(pair.Key, pair.Value) + '\n';
            }

            output += "\n@Controls\n";
            foreach (var pair in Control.GetDefault())
            {
                output += Util.MakeFieldPair(pair.Key, pair.Value) + '\n';
            }

            Util.WriteFile(FILENAME, output);

        }

        public void Load()
        {
            if (!File.Exists(FILENAME))
            {
                WriteDefaults();
            }
            Data data = new Data(FILENAME, '@');
            if (data.Count != 3)
            {
                throw new ParseError("Expected 3 sections, got {0}", data.Count);
            }
            for (int i = 0; i < data.Count; i += 1)
            {
                DataSection section = data[i];
                Matcher match = new Matcher(section.Tag);
                List<string[]> tokens = section.Tokens;
                if (match["audio"])
                {
                    Audio.Load(tokens);
                }
                else if (match["video"])
                {
                    Video.Load(tokens);
                }
                else if (match["controls"])
                {
                    Control.Load(tokens);
                }
                else
                {
                    throw new ParseError("Don't know how to handle section called {0} in {1}", section.Tag, FILENAME);
                }
            }
            

        }

        public void Apply()
        {

        }
    }
}
