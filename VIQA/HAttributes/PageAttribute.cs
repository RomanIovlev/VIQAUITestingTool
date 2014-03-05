using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class PageAttribute : NameAttribute
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public bool CheckUrl { get; set; }

        public bool CheckTitle { get; set; }

        public PageAttribute()
        {
            CheckUrl = !string.IsNullOrEmpty(Url);
            CheckTitle = !string.IsNullOrEmpty(Title);
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
