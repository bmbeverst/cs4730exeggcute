using System;
using System.Reflection;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading
{

    abstract class UnknownType
    {
        public static BindingFlags Flags = BindingFlags.NonPublic |
                                              BindingFlags.InvokeMethod |
                                              BindingFlags.Static |
                                              BindingFlags.Public;
        protected string value;
        protected FieldInfo fieldInfo;
        protected Type fieldType;
        protected Type[] innerTypes;

        protected virtual string parserName 
        {
            get { return "Parse"; }
        }

        protected abstract Type parserType { get; }

        protected virtual Type[] parserParamSignature
        {
            get { return new Type[] { typeof(string) }; }
        }

        protected virtual object[] parameters
        {
            get { return new object[] { value }; }
        }

        protected string parserClassName;

        protected UnknownType(FieldInfo fieldInfo, string value)
        {
            this.fieldInfo = fieldInfo;
            this.fieldType = fieldInfo.FieldType;
            this.value = value;
            initialize();
        }
        private void initialize()
        {
            innerTypes = fieldType.GetGenericArguments();
        }

        protected virtual MethodInfo getParserMethod()
        {
            return parserType.GetMethod(parserName, Flags, null, parserParamSignature, null);  
        }

        public void SetField(object obj)
        {
            MethodInfo parser = getParserMethod();
            if (parser == null)
            {
                throw new MethodNotFoundError(parserName, parserClassName);
            }
            object convertedValue = parser.Invoke(null, parameters);
            fieldInfo.SetValue(obj, convertedValue);
        }

        public static UnknownType MakeType(FieldInfo fieldInfo, string value)
        {
            Type fieldType = fieldInfo.FieldType;
           
            if (fieldType.IsGenericType)
            {
                Type innerType = fieldType.GetGenericArguments()[0];
                if (innerType.IsEnum)
                {
                    return new EnumType(fieldInfo, value);
                }
                else if (innerType.IsPrimitive)
                {
                    return new PrimitiveType(fieldInfo, value);
                }
                else if (!innerType.IsClass) // its a struct!
                {
                    return new BuiltinType(fieldInfo, value, false);
                }
                else
                {
                    throw new ParseError("How do I {0} without magic?", innerType);
                }
            }
            else
            {
                bool hasParser = fieldType.GetMethod("Parse", new Type[] { typeof(string) }) != null;
                if (hasParser)
                {
                    return new UserDefinedType(fieldInfo, value);
                }
                else
                {
                    return new BuiltinType(fieldInfo, value, true);
                }
            }
        }
    }

    class EnumType : UnknownType
    {
        protected override string parserName
        {
            get { return "ParseEnumNullable"; }
        }

        protected override Type parserType
        {
            get { return typeof(Util); }
        }

        protected override MethodInfo getParserMethod()
        {
            MethodInfo enumParser = parserType.GetMethod(parserName, parserParamSignature);
            return enumParser.MakeGenericMethod(innerTypes[0]);
        }

        public EnumType(FieldInfo fieldInfo, string value)
            : base(fieldInfo, value)
        {
            parserClassName = "Util";
        }
    }

    class PrimitiveType : UnknownType
    {
        protected override Type parserType
        {
            get { return innerTypes[0]; }
        }

        public PrimitiveType(FieldInfo fieldInfo, string value)
            : base(fieldInfo, value)
        {
            parserClassName = innerTypes[0].Name;
        }
    }

    class BuiltinType : UnknownType
    {

        protected string typeName;
        protected override string parserName
        {
            get { return "Parse" + typeName; }
        }

        protected override Type parserType
        {
            get { return typeof(Util); }
        }

        public BuiltinType(FieldInfo fieldInfo, string value, bool isNullable)
            : base(fieldInfo, value)
        {
            if (!isNullable)
            {
                typeName = innerTypes[0].Name;
                parserClassName = typeName;
            }
            else
            {
                typeName = fieldInfo.FieldType.Name;
                parserClassName = "Util";
            }
        }
    }



    class UserDefinedType : UnknownType
    {
        protected override Type parserType
        {
            get { return fieldType; }
        }

        public UserDefinedType(FieldInfo fieldInfo, string value)
            : base(fieldInfo, value)
        {

        }
    }



}
