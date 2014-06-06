using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ParallelTestsExamples.Site;
using VIQA.SiteClasses;

namespace ParallelTestsExamples.Tests
{
    [TestFixture]
    public class VIParallelWorkInTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        [Test]
        public void VIQAParallelTestsExample()
        {
            var listOfBrowsers = new [] { BrowserType.Chrome, BrowserType.Firefox/*, BrowserType.Chrome, BrowserType.Chrome, BrowserType.Chrome*/ };
            var tasks = listOfBrowsers.Select(GetTask).ToList();
            tasks.ForEach(task => task.Start());
            tasks.ForEach(task => task.Wait());
        }

        private static Task GetTask(BrowserType browser)
        {
            return new Task(() =>
            {
                var site = new YandexMarketSite { UseBrowser = browser };
                site.HomePage.Open();
                site.HomePage.SearchSection.SearchProduct("IPhone");
                site.ProductPage.CheckUrl(false);
                Thread.Sleep(2000);
            });
        }

    }
}
