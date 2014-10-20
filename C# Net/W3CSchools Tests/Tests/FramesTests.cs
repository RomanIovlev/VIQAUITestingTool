using NUnit.Framework;
using OpenQA.Selenium;
using W3CSchools_Tests.Site;

namespace W3CSchools_Tests.Tests
{

    [TestFixture]
    public class FramesTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        [Test]
        public void FramesTest()
        {
            var page = new W3CSite().W3CPageFrame;
            page.Open();
            page.StartLearningLink.Click();
            Assert.AreEqual(page.WebDriver.FindElements(By.XPath("//*[contains(text(), 'Example Explained')]")).Count, 1);
        }
    }
}
