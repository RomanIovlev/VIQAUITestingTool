using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium;

namespace VIQA.HtmlElements.Interfaces
{
    public interface IVIElement
    {
        string Name { get; }
        IWebDriver WebDriver { get; }
        bool IsPresent { get; }
        bool IsDisplayed { get; }
        IWebElement GetWebElement();
        By Locator { get; set; }
        int CashDropTime { get; set; }
    }
}
