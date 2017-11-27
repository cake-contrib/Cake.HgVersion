using HgVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HgVersion.Helpers;
using HgVersion.VCS;
using System.IO;
using Mercurial;
using Cake.HgVersion.VCS;
using System.Diagnostics;
using System.Threading;
using Cake.HgVersion;

namespace Cake.HgVersionTests
{
    public sealed class TestVesionContext : HgVersionContext, IDisposable
    {
        static TestVesionContext()
        {
            // added for solve concurrency problems
            if (!Client.CouldLocateClient)
                throw new MercurialMissingException("The Mercurial command line client could not be located");
        }
        
        public TestVesionContext() : base(CreateTempRepository())
        { }
        
        public void WriteTextAndCommit(string fileName, string content, string commitMessage = null)
        {
            var repository = (HgRepository)Repository;
            
            var path = Path.Combine(repository.Path, fileName);
            var fileInfo = new FileInfo(path);
            var message = GetCommitMessage(fileInfo, commitMessage);

            Directory.CreateDirectory(fileInfo.Directory.FullName);
            File.WriteAllText(fileInfo.FullName, content);

            Repository.AddRemove();
            Repository.Commit(message);
        }
        
        private static string GetCommitMessage(FileInfo fileInfo, string commitMessage)
        {
            if (!string.IsNullOrEmpty(commitMessage))
                return commitMessage;

            return fileInfo.Exists ? $"change {fileInfo.Name}" : $"create {fileInfo.Name}";
        }

        private static Repository CreateTempRepository()
        {
            var repoPath = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid()
                    .ToString()
                    .Replace("-", string.Empty)
                    .ToLowerInvariant());

            Directory.CreateDirectory(repoPath);
            
            var repository = new Repository(repoPath);
            repository.Init();

            return repository;
        }

        private static void DeleteTempRepository(IRepository repository)
        {
            for (int index = 1; index < 5; index++)
            {
                try
                {
                    if (Directory.Exists(repository.Path))
                        Directory.Delete(repository.Path, true);
                    break;
                }
                catch (DirectoryNotFoundException)
                { }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception while cleaning up repository directory: "
                                    + ex.GetType().Name + ": " +
                                    ex.Message);

                    Thread.Sleep(1000);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DeleteTempRepository(Repository);
                }
                
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
