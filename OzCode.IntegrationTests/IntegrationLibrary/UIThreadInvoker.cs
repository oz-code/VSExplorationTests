using System;
using System.Windows;

namespace OzCode.IntegrationTests.IntegrationLibrary
{
    public static class UIThreadInvoker
    {
        public static void Invoke(Action method)
        {
            Application.Current.Dispatcher.Invoke(method);
        }
    }
}