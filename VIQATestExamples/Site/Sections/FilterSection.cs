using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using VITestsProject.Data;

namespace VITestsProject.Site.Sections
{
    public class FilterSection : VIElement
    {
        [Name(Name = "Цена От"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]")]
        public readonly ITextArea TextFieldFrom = new TextField {
            FillRule = ToFillRule<Filter>(filter => (filter.CostRange != null) ? (int?)filter.CostRange.From : null)
        };

        [Name(Name = "Цена До"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]")]
        public readonly ITextArea TextFieldTo = new TextField {
            FillRule = ToFillRule<Filter>(filter => (filter.CostRange != null) ? (int?)filter.CostRange.To : null)
        };

        [Name(Name = "Wi-fi"), Locate(ByXPath = "//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input")]
        public readonly ICheckbox WiFiCheckbox = new Checkbox {
            FillRule = ToFillRule<Filter>(filter => filter.Wifi)
        };
        public readonly RadioButtons SensorScreenRadioButtons = new RadioButtons("Сенсорный экран",
            new RadioButton(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[text()='{0}']//..//input[@type='radio']"))) {
                DoViAction = new VIAction<Action<VIElement, string, Action>>((viElement, text, viAction) => {
                    VISite.Logger.Event(viElement.DefaultLogMessage(text));
                    if (!new VIElement("", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[contains(text(),'да'" +
                                                    ")]")).IsDisplayed)
                        new ClickableElement("Сенсорный экран", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//i")).Click();
                    viAction.Invoke();
                }),
                FillRule = ToFillRule<Filter>(filter => filter.SensorScreen)
        };

        public readonly ICheckList ProcessorTypesChecklist = new CheckList("Процессор", 
            new Checkbox(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//*[text()='{0} ']//..//input[@type='checkbox']"))) {
                DoViAction = new VIAction<Action<VIElement, string, Action>>((viElement, text, viAction) => {
                    VISite.Logger.Event(viElement.DefaultLogMessage(text));
                    if (!new Checkbox("'Процессор' Apple A4", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//*[contains(text(),'Apple A4')]")).IsDisplayed)
                        new ClickableElement("Процессор",
                            By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//i")).Click();
                    viAction.Invoke();
                    }),
                GetListOfValuesFunc = driver =>
                    driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//li//span"))
                        .Select(el => el.Text.Contains("U8500") ? el.Text.Replace("U8500", " U8500") : el.Text).ToList(),
                FillRule = ToFillRule<Filter>(filter => filter.ProcessorTypes)
        };

        public IButton ShowResultsButton { get { return new Button("Показать", "input[value='Показать']"); } }
    }
}
