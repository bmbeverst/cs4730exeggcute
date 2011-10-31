using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Exeggcute.src.scripting;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.loading
{
    abstract class Loadable
    {
        protected char DELIM = ':';
        public string Filename { get; protected set; }
        public List<string> methodFails = new List<string>();

        public Loadable(string filename)
        {
            this.Filename = filename;
        }

        protected virtual void loadFromFile(string filepath, bool verify)
        {
            List<string[]> lines = LinesFromFile(filepath);
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
                throw new ParseError("No implemented for properties, sorry");
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

        protected virtual List<string[]> LinesFromFile(string filepath)
        {
            string allText = Util.ReadAllText(filepath);
            return Util.CleanData(allText);
        }
        protected virtual Pair<FieldInfo, string> parse(string[] tokens, FieldInfo[] fields)
        {
            string name = tokens[0];
            FieldInfo info = null;
            foreach (FieldInfo field in fields)
            {
                if (equals(name, field.Name))
                {
                    info = field;
                }
            }
            if (info == null)
            {
                throw new ParseError("No field with name \"{0}\"", name);
            }
            string stringValue = tokens[1];
            Pair<FieldInfo, string> value = new Pair<FieldInfo, string>(info, stringValue);
            return value;
        }
        protected bool equals(string s0, string s1)
        {
            return Regex.IsMatch(s0, s1, RegexOptions.IgnoreCase);
        }

    }
}
/*LoadableType loadableType = helper.LoadType;
Type fieldType = info.FieldType;
string typeName = fieldType.Name;

BindingFlags flags = BindingFlags.NonPublic |
                     BindingFlags.InvokeMethod |
                     BindingFlags.Static |
                     BindingFlags.Public;

string value = helper.StringValue;
                
Type parserType;
object invoker = null;
object[] parameters = new object[] { value };
object parsedValue;

if (fieldType.IsGenericType)
{
    //HACK, for now we only look at the first type!
    Type innerType = fieldType.GetGenericArguments()[0];
    if (innerType.IsEnum)
    {
        parserName = "ParseEnumNullable";
        MethodInfo enumParser = utilType.GetMethod(parserName, typeSignature);
        method = enumParser.MakeGenericMethod(innerType);
        parserType = null;
        parsedValue = method.Invoke(invoker, parameters);
        info.SetValue(this, parsedValue);
        continue;
    }
    else if (innerType.IsPrimitive)
    {
        parserName = "Parse";
        parserType = innerType;
    }
    else if (!innerType.IsClass) // its a struct!
    {
        parserName = "Parse" + innerType.Name;
        parserType = utilType;
    }
    else
    {
        throw new ParseError("How do I {0} without magic?", innerType);
    }


}
else if (hasParserMethod)
{
    parserName = string.Format("Parse{0}", typeName);
    parserType = utilType;
}
else
{
    parserName = "Parse";
    parserType = fieldType;
}

method = parserType.GetMethod(parserName, flags, null, typeSignature, null);
parsedValue = method.Invoke(invoker, parameters);
info.SetValue(this, parsedValue);
}*/