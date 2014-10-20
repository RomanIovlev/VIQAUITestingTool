using System;
using System.Reflection;
using OpenQA.Selenium;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FrameAttribute : Attribute
    {
        private By FrameLocator;
        public string ById { set { FrameLocator = By.Id(value); } get { return ""; }}
        public string ByName { set { FrameLocator = By.Name(value); } get { return ""; } }
        public string ByClassName { set { FrameLocator = By.ClassName(value); } get { return ""; } }
        public string ByCssSelector { set { FrameLocator = By.CssSelector(value); } get { return ""; } }
        public string ByXPath { set { FrameLocator = By.XPath(value); } get { return ""; } }
        public string ByTagName { set { FrameLocator = By.TagName(value); } get { return ""; } }
        public string ByLinkText { set { FrameLocator = By.LinkText(value); } get { return ""; } }
        public string ByPartialLinkText { set { FrameLocator = By.PartialLinkText(value); } get { return ""; } }

        public FrameAttribute() { }
        public FrameAttribute(string frameId) { FrameLocator = By.Id(frameId); }

        public static By GetFrame(FieldInfo field)
        {
            var frame = field.GetCustomAttribute<FrameAttribute>(false);
            return frame != null ? frame.FrameLocator : null;
        }
    }
}
