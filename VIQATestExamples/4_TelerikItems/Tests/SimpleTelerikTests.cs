using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using VITestsProject._4_TelerikItems.Site;

namespace VITestsProject._4_TelerikItems.Tests
{
    [TestFixture]
    public class SimpleTelerikTests
    {
        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void TestCleanup()
        {
        }

        [Test]
        public void VITelerikSimpleTest()
        {
            var site = new TelerikSite();
            {
                var _ = site.ComboBoxPage;
                _.Open();
                _.T_ShirtFabrikDropDown.Select("Cotton");
                Assert.AreEqual(_.T_ShirtFabrikDropDown.Value, "Cotton");
            }
        }
    }
}
