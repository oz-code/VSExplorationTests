using System;
using System.IO;
using LibGit2Sharp;

namespace OzCode.IntegrationTests.Common
{
    internal class GitHelper
    {
        public static void CloneAndCheckout(ApplicationToDebug app)
        {
            if (app.GitCloneUrl == null) return;
            string repoPath = Path.Combine(Consts.TestAppsDirectory, $@"{app.Name}");

            if (!Directory.Exists(repoPath))
            {
                Directory.CreateDirectory(repoPath);
            }
            else if (IsAlreadyUpToDate(app, repoPath))
            {
                return;
            }

            Repository repo = Clone(app, repoPath);

            Console.WriteLine($"Checking out revision {app.GitCommitHash}");
            repo.Checkout(app.GitCommitHash);
        }

        private static Repository Clone(ApplicationToDebug app, string repoPath)
        {
            Repository.Clone(app.GitCloneUrl, repoPath,
                new CloneOptions
                {
                    OnProgress = ReportProgress,
                    OnTransferProgress = progress => ReportTransferProgress(progress)
                });

            var repo = new Repository(repoPath);

            var signature = new Signature("me", "someone@something.com", DateTimeOffset.Now);
            repo.Network.Pull(signature,
                new PullOptions {FetchOptions = new FetchOptions {OnProgress = ReportProgress}});
            return repo;
        }

        private static bool IsAlreadyUpToDate(ApplicationToDebug app, string repoPath)
        {
            try
            {
                return Repository.IsValid(repoPath) && new Repository(repoPath).Head.Tip.Sha == app.GitCommitHash;
            }
            catch (RepositoryNotFoundException)
            {
                return false;
            }
        }

        private static bool ReportProgress(string progress)
        {
            Console.WriteLine(progress);
            return true;
        }

        private static bool ReportTransferProgress(TransferProgress progress)
        {
            Console.WriteLine($"Received {progress.ReceivedBytes} of {progress.ReceivedBytes} bytes");
            return true;
        }
    }
}