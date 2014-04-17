using System;
using System.Collections.Generic;
using System.Linq;
using VIQA.HtmlElements;

namespace VIQA.Common
{
    public static class CommonExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable)
                action(element);
        }
        public static string FromNewLine(this string s)
        {
            return " " + Environment.NewLine + s;
        }

        public static string LineBreak(this string s)
        {
            return s + " " + Environment.NewLine;
        }
        public static string Print(this IEnumerable<string> list, string separator = ", ", string format = "{0}")
        {
            return (list != null) ? string.Join(separator, list.Select(el => string.Format(format, el))) : "";
        }

        public static T UseAsTemplate<T>(this T element, string id) where T : VIElement
        {
            element.TemplateId = id;
            return element;
        }
    }
}
