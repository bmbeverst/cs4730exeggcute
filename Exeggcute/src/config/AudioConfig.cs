using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Exeggcute.src.scripting;

namespace Exeggcute.src.config
{
    class AudioConfig : Config
    {
        private static readonly Dictionary<string, string> DEFAULT =
            new Dictionary<string,string>
            {
                {"Sound",  "On" },
                {"Master", "100" },
                {"Music",  "100" },
                {"Sfx",    "100" },
                {"Dialog", "100" }
            };

        private float master;
        public float MasterVolume 
        {
            get { return master * 0.5f; }
            protected set { master = value; }
        }

        private float sfx;
        public float SfxVolume 
        {
            get { return sfx * MasterVolume; }
            protected set { sfx = value; }
        }

        private float music;
        public float MusicVolume 
        {
            get { return music * MasterVolume; }
            protected set { music = value; }
        }

        private float dialog;
        public float Dialog 
        {
            get { return dialog * MasterVolume; }
            protected set { dialog = value; }
        }
        
        public bool Muted { get; protected set; }

        public override Dictionary<string, string> GetDefault()
        {
            return DEFAULT;
        }

        public override void Apply()
        {

        }

        public override void Set(string name, string value)
        {
            Matcher match = new Matcher(name);

            if (match["sound"])
            {
                match = new Matcher(value);
                if (match["on"])
                {
                    Muted = false;
                }
                else if (match["off"])
                {
                    Muted = true;
                }
                else
                {
                    goto FAIL;
                }
            }
            else
            {
                float newValue = normalize(value);
                if (match["master"])
                {
                    MasterVolume = newValue;
                }
                else if (match["sfx"])
                {
                    SfxVolume = newValue;
                }
                else if (match["music"])
                {
                    MusicVolume = newValue;
                }
                else if (match["dialog"])
                {

                    Dialog = newValue;
                }
                else
                {
                    goto FAIL;
                }
            }

            return;
        FAIL:
            throw new ParseError("failed to load sound setting");
        }

        private float normalize(string value)
        {
            float x = float.Parse(value);
            x /= 100.0f;
            return Util.Clamp(x, 0.0f, 1.0f);
        }


    }
}
