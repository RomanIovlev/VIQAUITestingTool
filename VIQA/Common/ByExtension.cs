using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.SiteClasses;

namespace VIQA.Common
{
    public static class ByExtension
    {
        public static Func<string, By> GetByFunc(this By by)
        {
            return MapByTypes.First(el => by.ToString().Contains(el.Key)).Value;
        }

        public static By FillByTemplate(this By by, params object[] args)
        {
            var byLocator = by.GetByLocator();
            int i = 0;
            if (args.Any(el => !byLocator.Contains("{" + i++ + "}")))
                throw VISite.Alerting.ThrowError("Bad locator template for element - " + byLocator + ". Missed " + "{" + (i - 1) + "}");
            return by.GetByFunc()(string.Format(by.GetByLocator(), args)); 
        }

        public static string GetByLocator(this By by)
        {
            var byAsString = by.ToString();
            var index = byAsString.IndexOf(": ") + 2;
            return byAsString.Substring(index);
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
