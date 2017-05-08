using System.IO;

namespace OzCode.IntegrationTests.Common
{
    /// <summary>
    /// Represents an open-source C# application we can Git-clone and 
    /// automatically step through for exploratory testing.
    /// </summary>
    public class ApplicationToDebug
    {
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string GitCommitHash { get; set; }
        public string SlnRelativePath { get; set; }
        public string BuildCommand { get; set; }
        public string StartupProjectName { get; set; }
        public bool IsInSourceCode { get { return GitCloneUrl == null; } }
        public string GetSlnFullPath()
        {
            if (IsInSourceCode)
            {
                return Path.GetFullPath(SlnRelativePath);
            }
            else
            {
                // The test app is an open source project we downloaded into the TestApps folder.
                return Path.Combine(Consts.TestAppsDirectory, Name, SlnRelativePath);
            }            
        }
    }
}
