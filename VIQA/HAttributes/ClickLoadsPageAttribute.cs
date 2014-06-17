using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ClickLoadsPageAttribute : Attribute
    {
        private readonly string _pageName;

        public ClickLoadsPageAttribute(string pageName = "")
        {
            _pageName = pageName;
        }
        public static string Handler(FieldInfo field)
        {
            var attr = field.GetCustomAttribute<ClickLoadsPageAttribute>(false);
            return (attr != null) 
                ? attr._pageName
                : null;
        }
    }
}

