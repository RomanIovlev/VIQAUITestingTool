using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements.Interfaces
{
    public interface IVIElement
    {
        string Name { get; }
        IWebElement GetWebElement();
        bool IsPresent { get; }
        bool IsDisplayed { get; }
        List<T> GetElements<T>();
        bool WaitElementState(Func<IWebElement, bool> waitFunc, IWebElement webElement = null);
        By Locator { get; set; }
        void SetWaitTimeout(int waitTimeoutInSec);
        VISite Site { get; set; }
        IWebDriver WebDriver { get; }
        void FillSubElements(Dictionary<IHaveValue, Object> values);
        void FillSubElements(Dictionary<string, Object> values);
        void FillSubElements(Object data);
        bool CompareValuesWith(Object data, Func<string, string, bool> compareFunc = null);
        void FillSubElement(string name, string value);
    }
}
