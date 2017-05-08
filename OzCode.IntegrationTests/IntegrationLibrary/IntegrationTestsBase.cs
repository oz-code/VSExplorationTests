using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using EnvDTE;
using EnvDTE80;
using EnvDTE90a;
using Microsoft.VisualStudio.Debugger.Interop.Internal;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudioTools.VSTestHost;
using OzCode.IntegrationTests.Aspects;
using OzCode.IntegrationTests.Common;
using Process = System.Diagnostics.Process;

namespace OzCode.IntegrationTests.IntegrationLibrary
{
    [TestClass]
    public abstract class IntegrationTestsBase
    {  
        private readonly EventWaitHandle _enteredBreakMode = new EventWaitHandle(false, EventResetMode.AutoReset);
        private readonly EventWaitHandle _enteredDesignMode = new EventWaitHandle(false, EventResetMode.AutoReset);
        private DebuggerEvents _debuggerEvents;
        private const int MaxIterations = 1;
        private const int MaxStepOvers = 1000;
        private DTE2 Dte => (DTE2) VSTestContext.DTE;


        private void SetUp()
        {
            _debuggerEvents = Dte.Events.DebuggerEvents;
            _debuggerEvents.OnEnterDesignMode += delegate { _enteredDesignMode.Set(); };
            _debuggerEvents.OnEnterBreakMode += delegate { _enteredBreakMode.Set(); };
        }

        protected void RunTest(ApplicationToDebug[] apps, params TestAspect[] aspects)
        {
            UIThreadInvoker.Invoke(SetUp);

            var testAspect = new CompositeTestAspect(aspects);

            foreach (var app in apps)
            {
                GitHelper.CloneAndCheckout(app);

                VSTestContext.DTE.Solution.Open(app.GetSlnFullPath());

                SetStartupProject(app.StartupProjectName);

                RunTest(testAspect);
            }

//            Process.Start("explorer.exe", Consts.TestArtifactsDirectory);
        }

        private void SetStartupProject(string projectName)
        {
            UIThreadInvoker.Invoke(() =>
            {
                Dte.Solution.SolutionBuild.StartupProjects = $@"{projectName}\{projectName}.csproj";
            });
        }

        private void RunTest(CompositeTestAspect testAspect)
        {
            testAspect.OnTestStarted();

            for (var i = 0; i < MaxIterations; i++)
            {
                StepOver();
                SetShowExternalCode();
                testAspect.OnStartDebugging();
                for (var step = 0; step < MaxStepOvers; step++)
                {
                    testAspect.OnBeforeStepOver();
					
                    if (!StepOver()) break;
                    
                    testAspect.OnAfterStepOver();                    
                }
                if (Dte.Debugger.CurrentMode != dbgDebugMode.dbgDesignMode) Dte.Debugger.Stop();
                testAspect.OnStopDebugging();
            }
            testAspect.OnTestFinished();
        }

        /// <summary>
        /// Perform a Step-Over (F10)
        /// </summary>
        /// <returns>True if the Step Over operation completed successfully, or false if either: 
        /// A) we stepped past the end of the program and landed in Design Mode
        /// B) we timed-out waiting for the step to complete
        /// </returns>
        private bool StepOver()
        {
            UIThreadInvoker.Invoke(() => { VSTestContext.DTE.ExecuteCommand("Debug.StepOver"); });
            var signaled = WaitHandle.WaitAny(new[] {_enteredBreakMode, _enteredDesignMode}, TimeSpan.FromMinutes(1));
            return signaled == 0;
        }
		
		       private void SetShowExternalCode()
        {
            UIThreadInvoker.Invoke(() =>
            {
                var debugger = (IDebuggerInternal) VSTestContext.ServiceProvider.GetService(typeof(SVsShellDebugger));
                debugger.SetDebuggerOption(DEBUGGER_OPTIONS.Option_ShowExternalCode, 1);
            });
        }
    }
}