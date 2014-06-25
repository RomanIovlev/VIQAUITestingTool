using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class FrameAttribute : Attribute
    {
        private readonly string _frameName;

        public FrameAttribute(string name) { _frameName = name; }

        public static string GetFrameName(FieldInfo field)
        {
            var name = field.GetCustomAttribute<FrameAttribute>(false);
            return name != null ? name._frameName : "";
        }

        public static string GetFrameName(Object obj)
        {
            var name = obj.GetType().GetCustomAttribute<FrameAttribute>(false);
            return name != null ? name._frameName : "";
        }
    }
}
