namespace OzCode.IntegrationTests.Common
{
    public static class AppsRepository
    {
        public static ApplicationToDebug[] GetAppsToDebug()
        {
            return new[]
            {
                new ApplicationToDebug
                {
                    Name = "ILSpy",
                    GitCloneUrl = "https://github.com/icsharpcode/ILSpy.git",
                    GitCommitHash = "7e8d010db27debd75ab71514beb868391e8fec53",
                    SlnRelativePath = "ILSpy.sln"
                }
            };
        }
    }
}