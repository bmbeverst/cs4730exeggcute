using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Exeggcute.src
{

    //Assembly assembly = Assembly.GetAssembly(typeof(ScriptBase));
    //GraphNode master = new GraphNode(assembly, assembly.GetTypes().ToList());
    class GraphNode : IComparable<GraphNode>
    {
        private const int MAX_DEPTH = 4;
        HashSet<GraphNode> depend = new HashSet<GraphNode>();
        Type myType;
        int depth;
        string name;
        static Dictionary<Type, bool> seen = new Dictionary<Type, bool>();
        public GraphNode(Assembly assembly, IEnumerable<Type> types)
        {
            this.myType = assembly.GetType();
            this.name = myType.Name;
            foreach (Type type in types)
            {
                if (choose(type))
                    depend.Add(new GraphNode(type));
            }
        }

        public GraphNode(Type myType)
            : this(myType, 0)
        {

        }

        private GraphNode(Type myType, int depth)
        {
            //Console.WriteLine("{0}{1}", "".PadLeft(4*depth), myType.Name);
            this.depth = depth;
            this.myType = myType;
            this.name = myType.Name;
            if (depth < MAX_DEPTH && !Regex.IsMatch(myType.Module.ToString(), "Xna"))
            {

                IEnumerable<FieldInfo> fields = myType.GetFields();
                HashSet<Type> toRecurse = new HashSet<Type>();
                foreach (FieldInfo field in fields)
                {
                    Type fieldType = field.FieldType;
                    if (fieldType.Equals(myType))
                    {
                        name += "--->";
                        continue;
                    }
                    else
                    {
                        if (choose(field.FieldType))
                            toRecurse.Add(field.FieldType);
                    }
                    
                }

                IEnumerable<PropertyInfo> props = myType.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    Type propertyType = prop.GetType();

                    if (propertyType.Equals(myType))
                    {
                        name += "--->";
                        continue;
                    }
                    else
                    {
                        if (choose(propertyType))
                            toRecurse.Add(prop.PropertyType);
                    }
                    
                }

                foreach (Type type in toRecurse)
                {
                    seen[type] = true;
                    depend.Add(new GraphNode(type, depth + 1));
                }
            }
        }

        public int CompareTo(GraphNode other)
        {
            return this.name.CompareTo(other.name);
        }

        private static bool choose(Type type)
        {
            return type.IsClass &&
                !type.IsGenericType &&
                !Regex.IsMatch(type.Module.ToString(), "CommonLanguageRuntime") &&
                !seen.ContainsKey(type);
        }

        public override string ToString()
        {
            string output = string.Format("{0}{1}\n", "".PadLeft(depth * 4), name);
            List<GraphNode> sorted = depend.ToList();
            sorted.Sort();
            foreach (GraphNode node in sorted)
            {
                output += node.ToString();
            }
            return output;
        }
    }
}
