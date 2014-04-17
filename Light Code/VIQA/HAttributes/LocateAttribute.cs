using System;
using System.Reflection;
using OpenQA.Selenium;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class LocateAttribute : Attribute
    {
        public string ById { get; set; }
        public string ByName { get; set; }
        public string ByClassName { get; set; }
        public string ByCssSelector { get; set; }
        public string ByXPath { get; set; }
        public string ByTagName { get; set; }
        public string ByLinkText { get; set; }
        public string ByPartialLinkText { get; set; }

        public By GetLocator()
        {
            if (ById != null)
                return By.Id(ById);
            if (ByName != null)
                return By.Name(ByName);
            if (ByClassName != null)
                return By.ClassName(ByClassName);
            if (ByCssSelector != null)
                return By.CssSelector(ByCssSelector);
            if (ByXPath != null)
                return By.XPath(ByXPath);
            if (ByTagName != null)
                return By.TagName(ByTagName);
            if (ByLinkText != null)
                return By.LinkText(ByLinkText);
            if (ByPartialLinkText != null)
                return By.TagName(ByPartialLinkText);
            return By.Id("Undefined locator");
        }

        public static By GetLocator(FieldInfo field)
        {
            var locates = field.GetCustomAttribute<LocateAttribute>(false);
            return locates != null ? locates.GetLocator() : null;
        }

        public static By GetLocator(Object obj)
        {
            var locates = obj.GetType().GetCustomAttribute<LocateAttribute>(false);
            return locates != null ? locates.GetLocator() : null;
        }
    }
}

