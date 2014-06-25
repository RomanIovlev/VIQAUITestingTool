using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class NameAttribute : Attribute
    {
        private readonly string _name;

        public NameAttribute(string name) { _name = name; }

        public static string GetName(FieldInfo field)
        {
            var name = field.GetCustomAttribute<NameAttribute>(false);
            return name != null ? name._name : "";
        }

        public static string GetName(Object obj)
        {
            var name = obj.GetType().GetCustomAttribute<NameAttribute>(false);
            return name != null ? name._name : "";
        }
    }
}
