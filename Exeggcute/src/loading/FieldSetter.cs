using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Exeggcute.src.loading
{
    class FieldSetter
    {
        public FieldInfo Field { get; protected set; }
        public string StringValue { get; protected set; }
        public FieldSetter(FieldInfo field, string value)
        {
            this.Field = field;
            this.StringValue = value;
        }
    }
}
