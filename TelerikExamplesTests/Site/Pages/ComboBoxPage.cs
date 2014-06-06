using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using VIQA.HtmlElements;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace TelerikExamplesTests.Site.Pages
{
    public class ComboBoxPage : VIPage
    {
        public IDropDown T_ShirtFabrikDropDown = new DropDown("T-SHIRT FABRIC",
        By.XPath("//h4[text()='T-shirt Fabric']//..//span[@aria-controls='fabric_listbox']"),
        () => new SelectItem("", By.XPath("//ul[@id='fabric_listbox']/li[text()='{0}']")) {
            IsSelectedFunc = selectItem => selectItem.GetWebElement().GetAttribute("class").Contains("k-state-selected")
            })
        {
            SelectAction = (selector, name) =>
            {
                selector.GetWebElement().Click();
                Thread.Sleep(300);
                selector.GetVIElementByName(name).Click();
            },
            ListOfValues = new List<string> { "Cotton", "Polyester", "Rib Knit" }
        };

    }
}
