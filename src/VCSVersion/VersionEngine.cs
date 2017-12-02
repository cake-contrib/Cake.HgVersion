using VCSVersion.Output;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion
{
    public sealed class VersionEngine
    {
        private readonly IVersionContext _context;

        public VersionEngine(IVersionContext context)
        {
            _context = context;
        }

        public VersionVariables Execute()
        {
            //var applicableBuildServers = BuildServerList.GetApplicableBuildServers();
            //var buildServer = applicableBuildServers.FirstOrDefault();

            var versionFinder = new VersionFinder();
            var semVersion = versionFinder.FindVersion(_context);

            var variablesBuilder = new VersionVariablesBuilder(semVersion, _context.Configuration);
            return variablesBuilder.Build();
        }
    }
}