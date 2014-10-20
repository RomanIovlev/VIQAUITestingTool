using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleTests.Site;
using VIQA.Common;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.SiteClasses;

namespace SimpleTests.Tests
{
    [TestFixture]
    public class VIQATests
    {
        [SetUp]
        public void Init() { VISite.KillAllRunWebDrivers(); }
        
        [TearDown]
        public void TestCleanup() { }

        [Test]
        public void VIQASimpleExampleTest()
        {
            var site = new YandexMarketSite { UseBrowser = BrowserType.Chrome };
            site.HomePage.Open();
            site.HomePage.SearchSection.SearchProduct("IPhone");
            CheckIPhone(site.WebDriver);
            VISite.KillAllRunWebDrivers();
        }

        
        [Test]
        public void VITestExampleLarge()
        {
            var site = new VISite(BrowserType.Chrome) { Domain = "http://market.yandex.ru/" };
            site.HomePage.Open();

            new TextField("Поле Поиска", By.XPath("//*[@class='search__table']//*[@id='market_search']"))
                .Input("IPhone");
            new Button("Кнопка 'Найти'", By.XPath("//*[@class='search__table']//*[contains(text(),'Найти')]//..//..//button"))
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
            new Button("Показать", "input[value='Показать']").Click();
            CheckIPhone(site.WebDriver);
            VISite.KillAllRunWebDrivers();
        }

        [Test]
        public void VITestExamplePageObjects()
        {
            YandexMarket.HomePage.Open();
            {
                var _ = YandexMarket.HomePage.SearchSection;
                _.SearchTextField.Input("IPhone");
                _.SearchButton.Click();
            }
            {
                var _ = YandexMarket.ProductPage.FilterSection;
                _.TextFieldFrom.Input("1000");
                _.TextFieldTo.Input("20000");
                _.WiFiCheckbox.Check();
                _.ShowResultsButton.Click();
            }

            YandexMarket.ProductPage.CheckUrl(PageCheckType.Contains);
            CheckIPhone(YandexMarket.WebDriver);
        }

        private void CheckIPhone(IWebDriver driver)
        {
            Assert.IsNotNull(driver.FindElement(By.XPath("//a[contains(text(),'iPhone')]")));
        }

        #region Common tests data
        private static readonly YandexMarketSite YandexMarket = new YandexMarketSite { UseBrowser = BrowserType.Chrome };

        #endregion
    }
}
