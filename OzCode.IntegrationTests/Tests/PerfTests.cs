using Microsoft.VisualStudio.TestTools.UnitTesting;
using OzCode.IntegrationTests.Aspects;
using OzCode.IntegrationTests.Common;
using OzCode.IntegrationTests.IntegrationLibrary;

namespace OzCode.IntegrationTests.Tests
{
    /// <summary>
    /// Example of how we do PerfTests @OzCode
    /// </summary>
    [TestClass]
    public class PerfTests : IntegrationTestsBase
    {
        [Ignore, TestMethod, HostType("VSTestHost")]
        [TestProperty("VSHive", "NoOzCode")]
        public void VanillaDebuggerToolWindowsClosed()
        {
            RunTest(
                AppsRepository.GetAppsToDebug(),
                new DotTraceAspect(),
                new AllToolWindowsClosedAspect()
                );
        }

        [Ignore, TestMethod, HostType("VSTestHost")]
        [TestProperty("VSHive", "NoOzCode")]
        public void CallstackAndAutosWindowsOpen()
        {
            RunTest(
                AppsRepository.GetAppsToDebug(),
                new DotTraceAspect(),
                new DebuggerToolWindowsOpenAspect()
                );
        }

        [Ignore, TestMethod, HostType("VSTestHost")]
        [TestProperty("VSHive", "OzCodeInstalled")]
        public void OzCodeInstalled()
        {
            RunTest(
                AppsRepository.GetAppsToDebug(),
		        new DotTraceAspect(),
                new AllToolWindowsClosedAspect()
                );
        }
    }
}