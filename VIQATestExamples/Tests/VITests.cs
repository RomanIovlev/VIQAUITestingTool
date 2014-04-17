using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using VIQA.HtmlElements;
using VIQA.SiteClasses;
using VITestsProject.Data;
using VITestsProject.Site;
using VITestsProject.Site.Pages;
using VITestsProject.Site.Sections;

namespace VITestsProject.Tests
{
    [TestFixture]
    public class VITests
    {

        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        private static YandexMarketSite _yandexMarket;
        public static YandexMarketSite YandexMarket = new YandexMarketSite(BrowserType.Chrome);
        public static SearchSection SearchSection = HomePage.SearchSection;
        
        [Test]
        public void BadSeleniumTestExample()
        {
            var driver = new ChromeDriver("..\\..\\Drivers");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
            driver.Navigate().GoToUrl("http://market.yandex.ru/");

            driver.FindElement(By.Id("search-input"))
                .SendKeys("IPhone");
            driver.FindElement(By.XPath("//*[@class='b-head-search']//*[contains(text(),'Найти')]//..//..//input"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]"))
                .SendKeys("1000");
            driver.FindElement(By.XPath("//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]"))
                .SendKeys("20000");
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//i"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[contains(text(),'да')]//..//input[@type='radio']"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[text()='Процессор']//..//i"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[text()='Процессор']//..//..//*[text()='Apple A4 ']//..//input[@type='checkbox']"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[text()='Процессор']//..//..//*[text()='Apple A5 ']//..//input[@type='checkbox']"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[text()='Процессор']//..//..//*[text()='Apple A6 ']//..//input[@type='checkbox']"))
                .Click();
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//*[text()='Процессор']//..//..//*[text()='Apple A7 ']//..//input[@type='checkbox']"))
                .Click();
            foreach (var el in driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//li")))
            {
                //el.
            }
            driver.FindElement(By.XPath("//*[@class='b-gurufilters']//input[@value='Показать']")).Click();

        }

        public static Button ContinueButton { get { return new Button("Далее к шагу 2", "#next-btn") { WithPageLoadAction = true }; } }

        [Test]
        public void SimpleVITestExample()
        {
            var site = new VISite(BrowserType.Chrome) { Domain = "http://market.yandex.ru/" };
            site.OpenHomePage();

            new TextField("Поле Поиска", By.Id("search-input"))
                .Input("IPhone");
            new Button("Кнопка 'Найти'", By.XPath("//*[@class='b-head-search']//*[contains(text(),'Найти')]//..//..//input"))
                .Click();
            new TextField("Цена От", By.XPath("//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]"))
                .Input("1000");
            new TextField("Цена До", By.XPath("//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]"))
                .Input("20000");

            new Checkbox("Wi-fi", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input"))
                .Check();

            new ClickableElement("Сенсорный экран", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//i"))
                .Click();
            new RadioButtons("Выбор Сенсорного Экрана", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Сенсорный экран')]//..//..//*[text()='{0}']//..//input[@type='radio']"))
                .Select("да");

            new ClickableElement("Процессор", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//i"))
                .Click();
            var a = site.WebDriver.FindElements(
                By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//li//span[contains(text(),'8500')]"));

            var processorsCheckBox = new CheckList("Выбор Процессора", By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//*[text()='{0} ']//..//input[@type='checkbox']"));
            processorsCheckBox.CheckGroup("Apple A4", "Apple A5", "Apple A6", "Apple A7", "MediaTek MT6572W", "MediaTek MT6515");
            processorsCheckBox.GetListOfValuesFunc = driver =>
                driver.FindElements(By.XPath("//*[@class='b-gurufilters']//*[contains(text(),'Процессор')]//..//..//li//span"))
                .Select(el => el.Text.Contains("U8500") ? el.Text.Replace("U8500", " U8500") : el.Text).ToList();
            processorsCheckBox.CheckGroup("Apple A5");
            new Button("Показать", "input[value='Показать']").Click();
        }

        public static void CheckProduct(Product product) { }

        public static Filter[] IPhoneFilters
        {
            get
            {
                return new[] { new Filter {
                    ShortSearchName = "IPhone", 
                    CostRange = new Range(1000, 2000), 
                    Wifi = true,
                    SensorScreen = SensorScreenTypes.да, 
                    ProcessorTypes = new []{"Apple A4", "Apple A5", "Apple A6", "MediaTek MT6572W", "MediaTek MT6515"}
                } };
            }
        }
        public static Product[] IPhoneProducts { get { return new[] { new Product("IPhone 5S 64Gb") }; } }

        [Test]
        public void VITestExamplePageObjects(
            [ValueSource("IPhoneProducts")] Product product,
            [ValueSource("IPhoneFilters")] Filter filter)
        {
            YandexMarket.HomePage.Open();
            {
                var _ = HomePage.SearchSection;

                _.SearchTextField.Input("IPhone");
                _.SearchButton.Click();
            }
            {
                var _ = ProductPage.FilterSection;

                _.TextFieldFrom.Input("1000");
                _.TextFieldTo.Input("2000");
                _.WiFiCheckbox.Check();
                _.SensorScreenRadioButtons.Select("да");
                _.ProcessorTypesChecklist.CheckGroup(new[]
                    {"Apple A4", "Apple A5", "Apple A6", "MediaTek MT6572W", "MediaTek MT6515"});
                _.ProcessorTypesChecklist.CheckGroup("Apple A5");
                _.ShowResultsButton.Click();
            }

            CheckProduct(product);
        }
        
        private static Filter ProductFilter
        {
            get { return new Filter { ProcessorTypes = new[] { "Apple A6", "Apple A7", "MediaTek MT6575" } }; }
        }

        [Test]
        public void VITestExampleWithDataForms(
            [ValueSource("IPhoneProducts")] Product product,
            [ValueSource("IPhoneFilters")] Filter filter)
        {
            YandexMarket.HomePage.Open();
            SearchSection.SearchProduct(filter.ShortSearchName);
            {
                var _ = ProductPage.FilterSection;
                _.FillForm(filter);
                _.FillElements(new Dictionary<string, object>
                {
                    {_.ProcessorTypesChecklist.Name, ProductFilter.ProcessorTypes}
                });
                _.ShowResultsButton.Click();
            }
            CheckProduct(product);
        }
    }
}
