using System;
using System.Reflection;

namespace VIQA.HAttributes
{    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FillFromFieldAttribute : Attribute
    {
        public readonly string FieldName;

        public FillFromFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public static string GetFieldName(FieldInfo field)
        {
            var fillFrom = field.GetCustomAttribute<FillFromFieldAttribute>(false);
            return fillFrom != null ? fillFrom.FieldName : "";
        }
    }
}
