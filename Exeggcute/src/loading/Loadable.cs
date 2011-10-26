using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Exeggcute.src.scripting;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;

namespace Exeggcute.src.loading
{
    abstract class Loadable
    {
        protected char DELIM = ':';
        protected virtual void loadFromFile(string filepath)
        {
            List<string[]> lines = LinesFromFile(filepath);
            loadFromTokens(lines);
        }

        protected virtual void loadFromTokens(List<string[]> tokenList)
        {
            List<FieldSetter> table = loadTable(tokenList);
            loadFields(table);
        }

        protected virtual List<FieldSetter> loadTable(List<string[]> tokenList)
        {
            Type thisType = this.GetType();
            PropertyInfo[] properties = thisType.GetProperties();
            if (properties.Length > 0) throw new ParseError("No implemented for properties, sorry");
            FieldInfo[] fields = thisType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            List<FieldSetter> table = new List<FieldSetter>();
            foreach (string[] tokens in tokenList)
            {
                FieldSetter setter = parse(tokens, fields);
                table.Add(setter);
            }
            return table;
        }

        protected virtual void loadFields(List<FieldSetter> table)
        {
            Type utilType = typeof(Util);

            foreach (FieldSetter setter in table)
            {
                FieldInfo info = setter.Field;
                string typeName = info.FieldType.Name;
                
                string value = setter.StringValue;
                Type fieldType = info.FieldType;
                string typeParseMethodName = "Parse";
                if (fieldType.IsGenericType)
                {
                    Type innerType = fieldType.GetGenericArguments()[0];
                    if (innerType.IsEnum)
                    {

                        string enumParseMethodName = "ParseEnumNullable";
                        MethodInfo enumParser = utilType.GetMethod(enumParseMethodName);
                        MethodInfo genericized = enumParser.MakeGenericMethod(innerType);
                        info.SetValue(this, genericized.Invoke(null, new object[] { value }));
                    }
                    else if (innerType.IsPrimitive)
                    {
                        string enumParseMethodName = "Parse";
                        MethodInfo primitiveParser = innerType.GetMethod(enumParseMethodName, new Type[] { typeof(string) });
                        info.SetValue(this, primitiveParser.Invoke(null, new object[] { value }));
                    }
                    
                }
                else if (fieldType.GetMethod(typeParseMethodName, new Type[] { typeof(string) }) == null)
                {
                    string utilParseMethodName = string.Format("Parse{0}", typeName);
                    
                    info.SetValue(this, utilType.InvokeMember(utilParseMethodName, BindingFlags.InvokeMethod, null, null, new object[] { value }));
                }
                else
                {
                    info.SetValue(this, fieldType.InvokeMember(typeParseMethodName, BindingFlags.InvokeMethod, null, null, new object[] { value }));
                }
            }
        }
        protected virtual List<string[]> LinesFromFile(string filepath)
        {
            string allText = Util.ReadAllText(filepath);
            return Util.CleanData(allText);
        }
        protected virtual FieldSetter parse(string[] tokens, FieldInfo[] fields)
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
            FieldSetter value = new FieldSetter(info, stringValue);
            return value;
        }
        protected bool equals(string s0, string s1)
        {
            return Regex.IsMatch(s0, s1, RegexOptions.IgnoreCase);
        }

    }
}
