using NUnit.Framework;
using VIQA.SiteClasses;

namespace W3CSchools_Tests
{
    [SetUpFixture]
    public class InitTests
    {

        [SetUp]
        public void InitTestRun()
        {
            VISite.KillAllRunWebDrivers();
        }

        [TearDown]
        public void TestRunCleanup()
        {
            VISite.DisposeAll();
            VISite.KillAllRunWebDrivers();
        }
    }
}