using System.IO;

namespace OzCode.IntegrationTests.Common
{
    public static class Consts
    {        
        private static string TestRootDirectory => Path.Combine(Path.GetTempPath(), "OzCodeIntegrationTests");
        public static string TestArtifactsDirectory => Path.Combine(TestRootDirectory, "TestArtifacts");
        public static string TestAppsDirectory => Path.Combine(TestRootDirectory, "AppsToDebug");
    }
}