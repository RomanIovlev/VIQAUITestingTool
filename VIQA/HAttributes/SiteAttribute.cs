using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class SiteAttribute : Attribute
    {
        public string Domain { get; set; }
        public bool UseCache { get; set; }
        public bool IsMain { get; set; }
        public bool DemoMode { get; set; }

        public SiteAttribute() { IsMain = true; UseCache = true; }

        public static SiteAttribute Get(FieldInfo field)
        {
            return field.GetCustomAttribute<SiteAttribute>(false);
        }

        public static SiteAttribute Get(Object obj)
        {
            return obj.GetType().GetCustomAttribute<SiteAttribute>(false);
        }
    }
}
