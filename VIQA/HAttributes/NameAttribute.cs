using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class NameAttribute : Attribute
    {
        public string Name;

        public NameAttribute(string name) { Name = name; }

        public static string GetName(FieldInfo field)
        {
            var name = field.GetCustomAttribute<NameAttribute>(false);
            return name != null ? name.Name : "";
        }

        public static string GetName(Object obj)
        {
            var name = obj.GetType().GetCustomAttribute<NameAttribute>(false);
            return name != null ? name.Name : "";
        }
    }
}
