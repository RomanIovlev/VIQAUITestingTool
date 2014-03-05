using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using VITestsProject.Data;

namespace VITestsProject.Site.Sections
{
    public class FilterSection : VISection
    {
        [Name(Name = "Цена От"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]")]
        public ITextArea TextFieldFrom = new TextField();

        [Name(Name = "Цена До"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]")]
        public ITextArea TextFieldTo = new TextField();

        [Name(Name = "Wi-fi"), Locate(ByXPath = "//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input")]
        public ICheckbox WiFiCheckbox = new Checkbox();

        [Name(Name = "Wi-fi"), Locate(ByXPath = "//*[@class='wifi-checkbox']")]
        public ICheckbox WiFiCheckbox1 = new Checkbox();

        [Name(Name = "Wi-fi"), Locate(ByClassName = "wifi-checkbox")]
        public Checkbox WiFiCheckbox2;

        public ICheckbox WiFiCheckbox3
            = new Checkbox("Wi-fi", "#wifi-checkbox");

        public ICheckbox WiFiCheckbox4
            = new Checkbox("Wi-fi", By.CssSelector("#wifi-checkbox"));

        public RadioButtons SensorScreenRadioButtons
        {
            get
            {
                return new RadioButtons("Сенсорный экран", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[text()='{0}']//..//input[@type='radio']"))
                {
                    VIAction = (viElement, text, viAction) => {
                        viElement.Site.Logger.Event(viElement.DefaultLogMessage(text));
                        if (!new VIElement("", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[contains(text(),'да')]")).IsDisplayed)
                            new ClickableElement("Сенсорный экран", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//i")).Click();
                        viAction.Invoke();
                        }
                };
            }
        }

        public ICheckList ProcessorTypesChecklist
        {
            get
            {
                return new CheckList("Процессор", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//*[text()='{0} ']//..//input[@type='checkbox']"))
                {
                    VIAction = (viElement, text, viAction) => {
                        viElement.Site.Logger.Event(viElement.DefaultLogMessage(text));
                        if (!new Checkbox("'Процессор' Apple A4", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//*[contains(text(),'Apple A4')]")).IsDisplayed)
                            new ClickableElement("Процессор",
                                By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//i")).Click();
                        viAction.Invoke();
                        },
                    GetListOfValuesFunc = driver =>
                        driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//li//span"))
                            .Select(el => el.Text.Contains("U8500") ? el.Text.Replace("U8500", " U8500") : el.Text).ToList()
                };
            }
        }

        public DataForm<Filter> ProductFilterForm
        {
            get
            {
                return
                    new DataForm<Filter>("Фильтр продукта", new Dictionary<ISetValue, Func<Filter, object>> {
                    { TextFieldFrom, _ => (_.CostRange != null) ? (int?)_.CostRange.From : null },
                    { TextFieldTo, _ => (_.CostRange != null) ? (int?)_.CostRange.To : null },
                    { WiFiCheckbox, _ => _.Wifi },
                    { SensorScreenRadioButtons, _ => _.SensorScreen },
                    { ProcessorTypesChecklist, _ => _.ProcessorTypes },
                });
            }
        }

        public IButton ShowResultsButton { get { return new Button("Показать", "input[value='Показать']"); } }
    }
}
