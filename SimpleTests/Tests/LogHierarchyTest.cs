using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.SiteClasses;

namespace SimpleTests.Tests
{
    [TestFixture]
    class LogIerarchyTest
    {
        public static readonly Func<string> LogPath = () => "/../.Logs/" + TestContext.CurrentContext.Test.FullName.Replace('.', '/') + "/" + VISite.RunId;

        private static VISite _mySite;
        public static VISite MySite
        {
            get
            {
                if (_mySite != null)
                    return _mySite;

                _mySite = new VISite(BrowserType.Chrome) { Domain = "http://market.yandex.ru/" };
                var logger = ((DefaultLogger)VISite.Logger);
                logger.LogDirectoryRoot = LogPath;
                logger.CreateFoldersForLogTypes = false;
                return _mySite;
            }
        }

        [TearDown]
        public void TestCleanup()
        {
            if (TestContext.CurrentContext.Result.State != TestState.Success)
                MySite.TakeScreenshot(path: LogPath(), outputFileName: "TestFail_Screenshot_" + VISite.RunId);
        }

        [Test, Ignore]
        public void ErrorTest()
        {
            MySite.HomePage.Open();
            MySite.WebDriver.FindElement(By.XPath("//something"));
        }

        [Test, Ignore]
        public void ErrorTest2()
        {
            MySite.HomePage.Open();
            MySite.WebDriver.FindElement(By.XPath("//something"));
        }
    }
}
