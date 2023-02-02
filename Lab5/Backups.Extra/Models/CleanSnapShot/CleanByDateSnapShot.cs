using Backups.Extra.Models.Cleaner;

namespace Backups.Extra.Models.CleanSnapShot;

public class CleanByDateSnapShot : ICleanSnapShot
{
    public DateTime TimeUntilWeStoreFiles { get; init; }

    public IExtraCleanerAlgorithm ToObject()
    {
        return new CleanByDate(TimeUntilWeStoreFiles);
    }
}