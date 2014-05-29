using System;
using System.Linq;
using OpenQA.Selenium;
using VIQA;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using Settings.VITestsProject.Data;

namespace Settings.VITestsProject.Site.Sections
{
    public class FilterSection : VIElement
    {
        [Name("Цена От")] 
        [Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]")] 
        [FillFromField("CostRange.From")]
        public readonly ITextField TextFieldFrom;

        [Name("Цена До"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]")]
        public readonly ITextField TextFieldTo = new TextField {
            FillRule = ToFillRule<Filter>(filter => (filter.CostRange != null) ? (int?)filter.CostRange.To : null)
        };

        [Name("Wi-fi"), Locate(ByXPath = "//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input"), FillFromField("Wifi")] 
        public readonly ICheckbox WiFiCheckbox;

        public readonly RadioButtons SensorScreenRadioButtons = new RadioButtons("Сенсорный экран",
            () => new RadioButton(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[text()='{0}']//..//input[@type='radio']"))) {
                DoViAction = new VIAction<Action<VIElement, string, Action>>((viElement, text, viAction) => {
                    VISite.Logger.Event(viElement.DefaultLogMessage(text));
                    if (!new VIElement("", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[contains(text(),'да'" + ")]")).IsDisplayed)
                        new ClickableElement("Сенсорный экран", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//i")).Click();
                    viAction.Invoke();
                }),
                FillRule = ToFillRule<Filter>(filter => filter.SensorScreen),
                GetAllElementsFunc = driver => 
                    driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//li//label")).ToDictionary(
                        webEl => webEl.FindElement(By.TagName("span")).Text, 
                        webEl => new RadioButton(webEl.FindElement(By.CssSelector("input[type=radio]"))))
        };

        [FillFromField("PlatformTypes")]
        public readonly ICheckList PlatformTypesChecklist = new CheckList("Платформа",
            () => new Checkbox(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Платформа')]//..//..//*[text()='{0} ']//..//input[@type='checkbox']")))
            {
                DoViAction = new VIAction<Action<VIElement, string, Action>>((viElement, text, viAction) => {
                    VISite.Logger.Event(viElement.DefaultLogMessage(text));
                    if (!new Checkbox("'Платформа' Android", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Платформа')]//..//..//*[contains(text(),'Android')]")).IsDisplayed)
                        new ClickableElement("Платформа",
                            By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Платформа')]//..//i")).Click();
                    viAction.Invoke();
                    }),
                GetAllElementsFunc = driver =>
                    driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Платформа')]//..//..//li//label")).ToDictionary(
                        webEl => webEl.FindElement(By.TagName("span")).Text, 
                        webEl => new Checkbox(webEl.FindElement(By.CssSelector("input[type=checkbox]")))),
                ClearAction = elements =>
                    elements.ForEach(el => { if (el.IsChecked()) el.Click(); })
        };

        public IButton ShowResultsButton { get { return new Button("Показать", "input[value='Показать']"); } }
    }
}
