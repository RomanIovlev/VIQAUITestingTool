using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Simple.VITestsProject.Tests.Simple
{
    [TestFixture]
    public class BadSeleniumTest
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        [Test]
        public void BadSeleniumTestExample()
        {
            var driver = new ChromeDriver("..\\..\\Drivers");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
            driver.Navigate().GoToUrl("http://market.yandex.ru/");

            driver.FindElement(By.XPath("//*[@class='b-search__input']//*[@class='b-form-input__input']"))
                .SendKeys("IPhone");
            driver.FindElement(By.XPath("//*[contains(text(),'Найти')]//..//..//input"))
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
    }
}
