using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading
{
    abstract class Loadable
    {
        protected char DELIM = ':';
        public string Filename { get; protected set; }
        public List<string> methodFails { get; protected set; }

        public Loadable(string filename)
        {
            this.Filename = filename;
            this.methodFails = new List<string>();
        }

        protected virtual List<string[]> linesFromFile(string filepath)
        {
            string allText = Util.ReadAllText(filepath);
            return Util.CleanData(allText);
        }

        protected virtual void loadFromFile(string filepath, bool verify)
        {
            List<string[]> lines = linesFromFile(filepath);
            loadFromTokens(lines, verify);
        }

        protected virtual void loadFromTokens(List<string[]> tokenList, bool verify)
        {
            List<Pair<FieldInfo, string>> pairs = loadPairs(tokenList);
            loadFields(pairs, verify);
        }

        protected virtual List<Pair<FieldInfo, string>> loadPairs(List<string[]> tokenList)
        {
            Type thisType = this.GetType();
            PropertyInfo[] properties = thisType.GetProperties();
            if (properties.Length > 0 && properties[0].Name != "Filename")
            {
                throw new ParseError("Not implemented for properties, sorry");
            }
            FieldInfo[] fields = thisType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            List<Pair<FieldInfo, string>> pairs = new List<Pair<FieldInfo, string>>();
            foreach (string[] tokens in tokenList)
            {
                Pair<FieldInfo, string> pair = parse(tokens, fields);
                pairs.Add(pair);
            }
            return pairs;
        }

        protected virtual void loadFields(List<Pair<FieldInfo, string>> pairs, bool verify)
        {
            bool methodNotFound = false;
            foreach (var pair in pairs)
            {
                FieldInfo info = pair.First;
                string value = pair.Second;

                UnknownType type = UnknownType.MakeType(info, value);
                try
                {
                    type.SetField(this);
                }
                catch (MethodNotFoundError mnf)
                {
                    methodNotFound = true;
                    methodFails.Add(mnf.Message);
                }

            }

            string errors = "";
            if (verify)
            {
                List<string> missing = getUninitialized();

                
                if (missing.Count > 0)
                {
                    errors += string.Format("    The following fields in {0} were uninitialized:\n", this.GetType().Name);
                    foreach (string missed in missing)
                    {
                        errors += string.Format("        {0}\n", missed);
                    }
                }
            }

            if (methodNotFound)
            {
                errors += string.Format("    Did not find the following expected methods:\n");
                foreach (string missed in methodFails)
                {
                    errors += string.Format("        {0}", missed);
                }
            }

            if (errors.Length > 0)
            {
                AssetManager.LogFailure(errors);
            }
        }

        protected virtual List<string> getUninitialized()
        {
            Type thisType = this.GetType();
            FieldInfo[] fields = thisType.GetFields();
            List<string> missing = new List<string>();
            foreach(FieldInfo info in fields)
            {
                if (info.DeclaringType == thisType &&
                    info.GetValue(this) == null)
                {
                    string error = string.Format("\"{0}\" of type \"{1}\"", info.Name, info.FieldType) ;
                    missing.Add(error);
                }
            }
            return missing;
        }


        protected virtual Pair<FieldInfo, string> parse(string[] tokens, FieldInfo[] fields)
        {
            string name = tokens[0];
            string stringValue = tokens[1];
            FieldInfo info = fields.FirstOrDefault(field => equals(name, field.Name));

            if (info == null)
            {
                throw new ParseError("No field with name \"{0}\"", name);
            }
            
            return new Pair<FieldInfo, string>(info, stringValue);
        }
        protected bool equals(string s0, string s1)
        {
            return Regex.IsMatch(s0, s1, RegexOptions.IgnoreCase);
        }

    }
}