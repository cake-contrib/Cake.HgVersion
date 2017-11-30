using VCSVersion.Configuration;
using VCSVersion.Helpers;
using VCSVersion.VCS;

namespace VCSVersion
{
    /// <summary>
    /// Current semantic versioning proccess context.
    /// </summary>
    public interface IVersionContext
    {
        /// <summary>
        /// Current repository.
        /// </summary>
        IRepository Repository { get; }

        /// <summary>
        /// File system utility.
        /// </summary>
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Contains the raw configuration.
        /// </summary>
        Config FullConfiguration { get; }

        /// <summary>
        /// Specific configuration based on the current context.
        /// </summary>
        EffectiveConfiguration Configuration { get; }

        /// <summary>
        /// Current branch.
        /// </summary>
        IBranchHead CurrentBranch { get; }

        /// <summary>
        /// Current commit.
        /// </summary>
        ICommit CurrentCommit { get; }

        /// <summary>
        /// Provides additional information about repository.
        /// </summary>
        IRepositoryMetadataProvider RepositoryMetadataProvider { get; }
    }
}