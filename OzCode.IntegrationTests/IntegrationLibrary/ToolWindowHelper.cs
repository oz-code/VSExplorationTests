using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudioTools.VSTestHost;

namespace OzCode.IntegrationTests.IntegrationLibrary
{
    internal static class ToolWindowHelper
    {
        public static void ShowDebuggerToolWindows()
        {
            CloseAllToolWindows();
            VSTestContext.DTE.ExecuteCommand("Debug.CallStack");
            VSTestContext.DTE.ExecuteCommand("Debug.Autos");
            VSTestContext.DTE.ExecuteCommand("Debug.DiagnosticTools.Show");
        }

        public static void CloseAllToolWindows()
        {
            foreach (Window window in VSTestContext.DTE.Windows)
            {
                if (window.Kind == "Tool" && window.Visible)
                {
                    window.Close();
                }
            }
        }
    }
}