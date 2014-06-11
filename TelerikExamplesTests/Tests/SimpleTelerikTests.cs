using NUnit.Framework;
using TelerikExamplesTests.Site;
using VIQA.SiteClasses;

namespace TelerikExamplesTests.Tests
{
    [TestFixture]
    public class SimpleTelerikTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        [Test]
        public void VITelerikSimpleTest()
        {
            var site = new TelerikSite { UseBrowser = BrowserType.Chrome };
            {
                var _ = site.ComboBoxPage;
                _.Open();
                _.T_ShirtFabrikDropDown.Select("Cotton");
                Assert.AreEqual(_.T_ShirtFabrikDropDown.Value, "Cotton");
            }
        }
    }
}
