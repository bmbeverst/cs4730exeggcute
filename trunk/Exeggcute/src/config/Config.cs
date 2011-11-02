using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Exeggcute.src.config
{
    abstract class Config
    {
        public virtual bool IsChanged { get; protected set; }
        public abstract void Apply();
        public abstract Dictionary<string, string> GetDefault();
        public virtual void Load(List<string[]> list)
        {
            foreach (string[] tokens in list)
            {
                Set(tokens[0], tokens[1]);
            }
            if (IsChanged)
            {
                Apply();
            }
        }
        public abstract void Set(string name, string value);

    }
}
