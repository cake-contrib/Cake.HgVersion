using Cake.HgVersion.VCS;
using HgVersion;
using HgVersion.Helpers;
using HgVersion.VCS;
using Mercurial;

namespace Cake.HgVersion
{
    /// <inheritdoc />
    public class HgVersionContext : IVersionContext
    {
        /// <inheritdoc />
        public IRepository Repository { get; }

        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        /// <summary>
        /// Creates an instance of <see cref="HgVersionContext"/>
        /// </summary>
        /// <param name="repository">Mercurial.Net <see cref="Mercurial.Repository"/></param>
        public HgVersionContext(Repository repository)
        {
            Repository = new HgRepository(repository);
            FileSystem = new FileSystem();
        }
    }
}
