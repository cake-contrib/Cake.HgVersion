using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using HgVersion;
using HgVersion.VCS;
using Mercurial;
using VCSVersion;
using VCSVersion.Configuration;
using VCSVersion.Helpers;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace HgVersionTests
{
    public sealed class TestVesionContext : IVersionContext, IDisposable
    {
        private HgVersionContext _context;
        private Repository _repository;
        
        static TestVesionContext()
        {
            // added for solve concurrency problems
            if (!Client.CouldLocateClient)
                throw new MercurialMissingException("The Mercurial command line client could not be located");
        }

        public TestVesionContext(bool inited = true)
        {
            _repository = CreateTempRepository(inited);
        }
        
        public void WriteTextAndCommit(string fileName, string content, string commitMessage = null)
        {
            var path = Path.Combine(_repository.Path, fileName);
            var fileInfo = new FileInfo(path);
            var message = GetCommitMessage(fileInfo, commitMessage);

            Directory.CreateDirectory(fileInfo.Directory.FullName);
            File.WriteAllText(fileInfo.FullName, content);

            _repository.AddRemove();
            _repository.Commit(message);
        }

        private IVersionContext GetContext()
        {
            return LazyInitializer.EnsureInitialized(ref _context, () => new HgVersionContext(_repository));
        }
        
        private static string GetCommitMessage(FileInfo fileInfo, string commitMessage)
        {
            if (!string.IsNullOrEmpty(commitMessage))
                return commitMessage;

            return fileInfo.Exists ? $"change {fileInfo.Name}" : $"create {fileInfo.Name}";
        }

        private static Repository CreateTempRepository(bool inited)
        {
            var repoPath = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid()
                    .ToString()
                    .Replace("-", string.Empty)
                    .ToLowerInvariant());

            Directory.CreateDirectory(repoPath);
            
            var repository = new Repository(repoPath);

            if (inited)
            {
                repository.Init();
            }

            return repository;
        }

        private static void DeleteTempRepository(Repository repository)
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
        private bool _disposedValue = false;

        void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DeleteTempRepository(_repository);
                }
                
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region IVersionContext implementation
        IRepository IVersionContext.Repository => GetContext().Repository;
        IFileSystem IVersionContext.FileSystem => GetContext().FileSystem;
        Config IVersionContext.FullConfiguration => GetContext().FullConfiguration;
        EffectiveConfiguration IVersionContext.Configuration => GetContext().Configuration;
        IBranchHead IVersionContext.CurrentBranch => GetContext().CurrentBranch;
        ICommit IVersionContext.CurrentCommit => GetContext().CurrentCommit;
        IRepositoryMetadataProvider IVersionContext.RepositoryMetadataProvider => GetContext().RepositoryMetadataProvider;
        bool IVersionContext.IsCurrentCommitTagged => GetContext().IsCurrentCommitTagged;
        SemanticVersion IVersionContext.CurrentCommitTaggedVersion => GetContext().CurrentCommitTaggedVersion;
        #endregion
    }
}
