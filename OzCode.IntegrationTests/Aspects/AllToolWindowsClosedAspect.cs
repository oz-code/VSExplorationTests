using OzCode.IntegrationTests.IntegrationLibrary;

namespace OzCode.IntegrationTests.Aspects
{
    public class AllToolWindowsClosedAspect : TestAspect
    {
        public override void OnStartDebugging()
        {
            UIThreadInvoker.Invoke(ToolWindowHelper.CloseAllToolWindows);
        }
    }
}