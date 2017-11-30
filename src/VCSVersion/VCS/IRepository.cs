using System;
using System.Collections.Generic;

namespace VCSVersion.VCS
{
    /// <summary>
    /// Abstraction for a VCS repository.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the path of the repository root.
        /// </summary>
        /// <value>The path of the repository root.</value>
        string Path { get; }

        /// <summary>
        /// Gets all the commits from the log.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="ICommit" /> instances.
        /// </returns>
        IEnumerable<ICommit> Log();
        
        /// <summary>
        /// Gets the commits from the log.
        /// </summary>
        /// <param name="query">The query that specifies the set of commits from the log.</param>
        /// <returns>A collection of <see cref="ICommit" /> instances.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="query"/> is <c>null</c>.</para>
        /// </exception>
        IEnumerable<ICommit> Log(ILogQuery query);

        /// <summary>
        /// Gets the commits from the log.
        /// </summary>
        /// <param name="config">
        /// Configuration method for the <see cref="ILogQuery"/> that specifies the set of commits
        /// from the log.
        /// </param>
        /// <returns>A collection of <see cref="ICommit" /> instances.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="config"/> is <c>null</c>.</para>
        /// </exception>
        IEnumerable<ICommit> Log(Func<ILogQueryBuilder, ILogQuery> config);

        /// <summary>
        /// Gets current repository heads.
        /// </summary>
        /// <returns>A collection of <see cref="ICommit" /> instances.</returns>
        IEnumerable<ICommit> Heads();

        /// <summary>
        /// Gets the current branch.
        /// </summary>
        /// <returns>The current branch.</returns>
        IBranchHead CurrentBranch();
        
        /// <summary>
        /// Gets the current commit.
        /// </summary>
        /// <returns>The current commit.</returns>
        ICommit CurrentCommit();
        
        /// <summary>
        /// Gets the repository branches.
        /// </summary>
        /// <returns>A collection of <see cref="IBranchHead"/> objects.</returns>
        IEnumerable<IBranchHead> Branches();

        /// <summary>
        /// Retrieves the tip commit.
        /// </summary>
        /// <returns>
        /// The <see cref="ICommit"/> of the tip commit.
        /// </returns>
        ICommit Tip();

        /// <summary>
        /// Add or remove a tag for a <see cref="ICommit"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the tag.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is <c>null</c> or empty.</para>
        /// </exception>
        void Tag(string name);

        /// <summary>
        /// List repository tags.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="ITag"/> objects for the existing tags in the repository.
        /// </returns>
        IEnumerable<ITag> Tags();

        /// <summary>
        /// Add all new files, delete all missing files.
        /// </summary>
        void AddRemove();

        /// <summary>
        /// Commits the specified files or all outstanding changes to the repository.
        /// </summary>
        /// <param name="message">The commit message to use.</param>
        /// <returns>
        /// The hash of the new commit.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="message" /> is <c>null</c> or empty.</para>
        /// </exception>
        string Commit(string message);

        /// <summary>
        /// Retrieve the parents of the current commit.
        /// </summary>
        /// <param name="commit">The current commit</param>
        /// <returns>
        /// A collection of <see cref="ICommit"/> objects, the parents of the current commit.
        /// </returns>
        IEnumerable<ICommit> Parents(ICommit commit);
    }
}