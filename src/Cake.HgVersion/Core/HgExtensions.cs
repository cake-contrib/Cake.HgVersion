using System.Linq;
using Mercurial;

// ReSharper disable MemberCanBePrivate.Global

namespace Cake.HgVersion.Core
{
    /// <summary>
    /// Common Mercurial.NET extension methods
    /// </summary>
    public static class HgExtensions
    {
        /// <summary>
        /// Retrieve first changeset by revision
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="revision">Revision</param>
        /// <returns></returns>
        public static Changeset Changeset(this Repository repository, RevSpec revision)
        {
            var id = repository.Identify(new IdentifyCommand()
                .WithAdditionalArgument($"--rev {revision}"));

            var log = repository.Log(new LogCommand()
                .WithRevision(id)
                .WithAdditionalArgument("--limit 1"));

            return log.First();
        }

        /// <summary>
        /// Retrieve first parent of changeset.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="changeset">Changeset</param>
        /// <returns></returns>
        public static Changeset Parent(this Repository repository, Changeset changeset)
        {
            return repository.Parent(changeset.Revision);
        }

        /// <summary>
        /// Retrieve first parent of revision.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="revision">Revision</param>
        /// <returns></returns>
        public static Changeset Parent(this Repository repository, RevSpec revision)
        {
            return repository
                .Parents(new ParentsCommand().WithRevision(revision))
                .First();
        }
    }
}
