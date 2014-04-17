using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace VIQA.Common
{
    public static class ByExtension
    {
        public static Func<string, By> GetByFunc(this By by)
        {
            return MapByTypes.First(el => by.ToString().Contains(el.Key)).Value;
        }

        public static string GetByLocator(this By by)
        {
            var byAsString = by.ToString();
            var index = byAsString.IndexOf(": ") + 2;
            return byAsString.Substring(index);
        }

        public static By SetLocatorTemplateValue(this By by, string value)
        {
            return by.GetByFunc().Invoke(string.Format(by.GetByLocator(), value));
        }

        private static readonly Dictionary<string, Func<string, By>> MapByTypes = new Dictionary<string, Func<string, By>>
        {
            {"CssSelector", By.CssSelector},
            {"ClassName", By.ClassName},
            {"Id", By.Id},
            {"LinkText", By.LinkText},
            {"Name", By.Name},
            {"PartialLinkText", By.PartialLinkText},
            {"TagName", By.TagName},
            {"XPath", By.XPath},
        };
    }

}
