using Backups.Extra.Models.Cleaner;

namespace Backups.Extra.Models.CleanSnapShot;

public interface ICleanSnapShot
{
    public IExtraCleanerAlgorithm ToObject();
}