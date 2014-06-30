using System;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class LocatorAttribute : Attribute
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

        public static By GetLocatorFromFindBy(FindsByAttribute fbAttr)
        {
            switch (fbAttr.How)
            {
                case How.Id:
                    return By.Id(fbAttr.Using);
                case How.Name:
                    return By.Name(fbAttr.Using);
                case How.ClassName:
                    return By.ClassName(fbAttr.Using);
                case How.CssSelector:
                    return By.CssSelector(fbAttr.Using);
                case How.XPath:
                    return By.XPath(fbAttr.Using);
                case How.TagName:
                    return By.TagName(fbAttr.Using);
                case How.LinkText:
                    return By.LinkText(fbAttr.Using);
                case How.PartialLinkText:
                    return By.PartialLinkText(fbAttr.Using);
                default:
                    return By.Id("Undefined locator");
            }
        }


        public static By GetLocator(FieldInfo field)
        {
            var locates = field.GetCustomAttribute<LocatorAttribute>(false);
            return locates != null ? locates.GetLocator() : null;
        }

        public static By GetLocatorFomFindsBy(FieldInfo field)
        {
            var locates = field.GetCustomAttribute<FindsByAttribute>(false);
            return locates != null ? LocatorAttribute.GetLocatorFromFindBy(locates) : null;
        }

        public static By GetLocator(Object obj)
        {
            var locates = obj.GetType().GetCustomAttribute<LocatorAttribute>(false);
            return locates != null ? locates.GetLocator() : null;
        }
    }
}

