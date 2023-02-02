using Backups.Extra.Models.Cleaner;

namespace Backups.Extra.Models.CleanSnapShot;

public class CleanByCountSnapShot : ICleanSnapShot
{
    public int CountRestorePoints { get; init; }

    public IExtraCleanerAlgorithm ToObject()
    {
        return new CleanByCount(CountRestorePoints);
    }
}