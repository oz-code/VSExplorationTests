using OzCode.IntegrationTests.IntegrationLibrary;

namespace OzCode.IntegrationTests.Aspects
{
    public class CompositeTestAspect : TestAspect
    {
        private readonly TestAspect[] _aspects;

        public CompositeTestAspect(TestAspect[] aspects)
        {
            _aspects = aspects;
        }

        public override void OnTestStarted()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnTestStarted();
            }
        }

        public override void OnStartDebugging()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnStartDebugging();
            }
        }

        public override void OnBeforeStepOver()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnBeforeStepOver();
            }
        }

        public override void OnAfterStepOver()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnAfterStepOver();
            }
        }

        public override void OnStopDebugging()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnStopDebugging();
            }
        }

        public override void OnTestFinished()
        {
            foreach (var aspect in _aspects)
            {
                aspect.OnTestFinished();
            }
        }
    }
}