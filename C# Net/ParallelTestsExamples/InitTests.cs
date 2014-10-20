using NUnit.Framework;
using VIQA.SiteClasses;

namespace ParallelTestsExamples
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