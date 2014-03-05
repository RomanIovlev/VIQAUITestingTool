using System;
using OpenQA.Selenium;

namespace VIQA.Common
{
    public class HtmlLocator
    {
        public By ByLocator;
        public bool IsTemplate;
        public string LocatorAsString { get { return ByLocator.GetByLocator();} }
        public Func<string, By> LocatorType { get { return ByLocator.GetByFunc(); } }

    }
}
