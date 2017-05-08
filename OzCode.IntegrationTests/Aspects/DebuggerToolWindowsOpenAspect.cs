using OzCode.IntegrationTests.IntegrationLibrary;

namespace OzCode.IntegrationTests.Aspects
{
    public class DebuggerToolWindowsOpenAspect : TestAspect
    {
        public override void OnStartDebugging()
        {
            UIThreadInvoker.Invoke(ToolWindowHelper.ShowDebuggerToolWindows);
        }
    }
}