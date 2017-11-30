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

        public SemanticVersion Execute()
        {
            var baseCalculator = new BaseVersionCalculator(
                new FallbackBaseVersionStrategy());
            
            var versionCalculator = new NextVersionCalculator(baseCalculator);
            return versionCalculator.CalculateVersion(_context);
        }
    }
}