using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using JetBrains.Profiler.Windows.Api;
using JetBrains.Profiler.Windows.SelfApi;
using JetBrains.Profiler.Windows.SelfApi.Config;
using OzCode.IntegrationTests.Common;
using OzCode.IntegrationTests.IntegrationLibrary;

namespace OzCode.IntegrationTests.Aspects
{
    public class DotTraceAspect : TestAspect
    {
        const string DotTraceSdkUrl = "https://download.jetbrains.com/resharper/JetBrains.Profiler.SelfSdk.2016.1.2.zip";
        private string DotTraceOutputDirectory => Path.Combine(Consts.TestArtifactsDirectory, "dotTrace");
        private string DotTraceSdkDirectory => Path.Combine(Consts.TestArtifactsDirectory, "dotTraceSdk");

        private int _counter;

        public override void OnTestStarted()
        {
            EnsureDotTraceSdkInstalled();

            var config = new SaveSnapshotProfilingConfig
            {
                ProfilingControlKind = ProfilingControlKind.Api,
                TempDir = Path.GetTempPath(),
                SaveDir = DotTraceOutputDirectory,
                RedistDir = DotTraceSdkDirectory,
                ProfilingType = ProfilingType.Performance,
                SnapshotFormat = SnapshotFormat.Uncompressed,
                HasCoreLog = true,
                ListFile = $@"{DotTraceOutputDirectory}\snapshot_list.xml"
            };
            if (!Directory.Exists(DotTraceOutputDirectory)) Directory.CreateDirectory(DotTraceOutputDirectory);
            SelfAttach.Attach(config);
            while (SelfAttach.State != SelfApiState.Active)
                Thread.Sleep(250); // wait until API starts
        }

        private void EnsureDotTraceSdkInstalled()
        {
            if (!Directory.Exists(DotTraceSdkDirectory))
            {
                Directory.CreateDirectory(DotTraceSdkDirectory);
                Console.WriteLine("Downloading dotTrace.");               
                string zipFile = Path.Combine(Consts.TestArtifactsDirectory, "dotTraceSdk.zip");

                using (var client = new WebClient())
                {
                    client.DownloadFile(DotTraceSdkUrl, zipFile);
                }

                ZipFile.ExtractToDirectory(zipFile,DotTraceSdkDirectory);
            }
            
        }

        public override void OnStartDebugging()
        {
            _counter++;
            if (_counter == 2) // Only start profiling after "warm-up" round
            {
                PerformanceProfiler.Begin();
                PerformanceProfiler.Start();
            }
        }

        public override void OnTestFinished()
        {
            PerformanceProfiler.Stop();
            PerformanceProfiler.EndSave();
        }
    }
}