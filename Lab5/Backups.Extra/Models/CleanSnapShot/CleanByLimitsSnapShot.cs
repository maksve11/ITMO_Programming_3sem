using Backups.Extra.Models.Cleaner;
using Backups.Extra.Tools;

namespace Backups.Extra.Models.CleanSnapShot;

public class CleanByLimitsSnapShot : ICleanSnapShot
{
    public List<ICleanSnapShot>? Algorithms { get; init; }

    public IExtraCleanerAlgorithm ToObject()
    {
        if (Algorithms != null) return new CleanByLimits(Algorithms.Select(x => x.ToObject()).ToList());
        throw new BackupsExtraException();
    }
}