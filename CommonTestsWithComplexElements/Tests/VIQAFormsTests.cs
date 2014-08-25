using System.Collections.Generic;
using CommonTestsWithComplexElements.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.SiteClasses;
using CommonTestsWithComplexElements.Site;

namespace CommonTestsWithComplexElements.Tests
{
    [TestFixture]
    public class VIQAFormsTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }
        
        [Test]
        public void VITestExampleWithDataForms([ValueSource("IPhoneFilters")] Filter filter)
        {
            {
                var _ = YandexMarket.HomePage;
                _.Open();
                _.SearchSection.SearchProduct(filter.ShortSearchName);
            }
            {
                var _ = YandexMarket.ProductPage.FilterSection;
                _.FillSubElements(filter);
                // Check that all fields correctly filled
                string compareResult;
                Assert.IsTrue(_.CompareValuesWith(filter, out compareResult), compareResult);
                // or with custom output
                _.CompareValuesWith(filter, out compareResult, (a, e) =>
                {
                    VISite.Logger.Event(string.Format("Compare Actual: {0}; Expected: {1}", a, e));
                    Assert.AreEqual(a, e);
                    return true;
                });
                _.FillSubElements(new Dictionary<string, object>
                {
                    {_.PlatformTypesChecklist.Name, ProductFilter.PlatformTypes}
                });
                _.ShowResultsButton.Click();
            }
            Assert.IsNotNull(YandexMarket.WebDriver.FindElement(By.XPath("//a[contains(text(),'iPhone')]")));
        }

        #region Common tests data

        private static readonly YandexMarketSite YandexMarket = new YandexMarketSite { UseBrowser = BrowserType.Chrome};


        private static Filter[] IPhoneFilters
        {
            get
            {
                return new[] { new Filter {
                    ShortSearchName = "IPhone", 
                    CostRange = new Range(1000, 2000), 
                    Wifi = true,
                    SensorScreen = SensorScreenTypes.да, 
                    PlatformTypes = new []{"Android", "iOS", "Symbian", "Series 60" }
                } };
            }
        }

        #endregion

        private static Filter ProductFilter
        {
            get { return new Filter { PlatformTypes = new[] { "BlackBerry OS", "Symbian", "Series 60", "Series 40" } }; }
        }


    }

}
