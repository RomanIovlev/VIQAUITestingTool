using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class PageAttribute : Attribute
    {
        public string Url { get; set; }

        public string Title { get; set; }
        
        public bool IsUrlCheckContainsNeeded { get; set; }
        public bool IsTitleCheckContainsNeeded { get; set; }
        public bool IsUrlCheckEqualNeeded { get; set; }
        public bool IsTitleCheckEqualNeeded { get; set; }

        public PageAttribute()
        {
            IsUrlCheckContainsNeeded = !string.IsNullOrEmpty(Url);
            IsTitleCheckContainsNeeded = !string.IsNullOrEmpty(Title);
        }

        public static PageAttribute Handler(FieldInfo field)
        {
            return field.GetCustomAttribute<PageAttribute>(false);
        }

        public static PageAttribute Handler(Object obj)
        {
            return obj.GetType().GetCustomAttribute<PageAttribute>(false);
        }
    }
}
