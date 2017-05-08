namespace OzCode.IntegrationTests.IntegrationLibrary
{
    public abstract class TestAspect
    {
        public virtual void OnTestStarted()
        {
        }

        public virtual void OnStartDebugging()
        {
        }

        public virtual void OnBeforeStepOver()
        {
        }

        public virtual void OnAfterStepOver()
        {
        }

        public virtual void OnStopDebugging()
        {
        }

        public virtual void OnTestFinished()
        {
        }
    }
}