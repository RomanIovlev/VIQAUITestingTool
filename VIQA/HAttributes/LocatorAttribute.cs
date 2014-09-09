using System;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class LocatorAttribute : Attribute
    {
        private By Locator;
        public string ById { set { Locator = By.Id(value); } get { return ""; } }
        public string ByName { set { Locator = By.Name(value); } get { return ""; } }
        public string ByClassName { set { Locator = By.ClassName(value); } get { return ""; } }
        public string ByCssSelector { set { Locator = By.CssSelector(value); } get { return ""; } }
        public string ByXPath { set { Locator = By.XPath(value); } get { return ""; } }
        public string ByTagName { set { Locator = By.TagName(value); } get { return ""; } }
        public string ByLinkText { set { Locator = By.LinkText(value); } get { return ""; } }
        public string ByPartialLinkText { set { Locator = By.PartialLinkText(value); } get { return ""; } }

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
            var locator = field.GetCustomAttribute<LocatorAttribute>(false);
            return locator != null ? locator.Locator : null;
        }

        public static By GetLocatorFomFindsBy(FieldInfo field)
        {
            var locator = field.GetCustomAttribute<FindsByAttribute>(false);
            return locator != null ? GetLocatorFromFindBy(locator) : null;
        }

        public static By GetLocator(Object obj)
        {
            var locator = obj.GetType().GetCustomAttribute<LocatorAttribute>(false);
            return locator != null ? locator.Locator : null;
        }
    }
}

