using NUnit.Framework;
using VIQA.SiteClasses;

namespace CommonTestsWithComplexElements
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