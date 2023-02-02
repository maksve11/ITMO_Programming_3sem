using Backups.Extra.Entities;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Models.CleanSnapShot;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public interface IExtraCleanerAlgorithm
{
    List<RestorePointExtra> FindPointsToClear(BackupTaskExtra extraBackupTask);
    void CleanPoints(BackupTaskExtra extraBackupTask);
    ICleanSnapShot ToSnapshot();
}