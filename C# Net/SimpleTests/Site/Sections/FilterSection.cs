using OpenQA.Selenium.Support.PageObjects;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;

namespace SimpleTests.Site.Sections
{
    public class FilterSection : VIElement
    {
        [Name("Цена От")]
        [Locator(ByXPath = ".//*[@class='b-gurufilters__filter-inputs']//input[contains(@id,'-0')]")] 
        public readonly ITextField TextFieldFrom;

        [Name("Цена До"), FindsBy(How = How.XPath, Using = ".//*[@class='b-gurufilters__filter-inputs']//input[contains(@id,'-1')]")]
        public readonly ITextField TextFieldTo;

        [Name("Wi-fi"), Locator(ByXPath = ".//*[contains(text(),'Wi-Fi')]//..//input")]
        public readonly ICheckbox WiFiCheckbox;

        public IButton ShowResultsButton = new Button("Показать", "input[value=Показать]"); 
    }
}
