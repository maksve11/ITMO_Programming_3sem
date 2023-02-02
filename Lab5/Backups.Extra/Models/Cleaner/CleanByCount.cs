using System.Collections.Immutable;
using Backups.Extra.Entities;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Models.CleanSnapShot;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public class CleanByCount : IExtraCleanerAlgorithm
{
    private readonly int _countRestorePoints;

    public CleanByCount(int countRestorePoints)
    {
        if (countRestorePoints < 0) throw new BackupsExtraException("Invalid count of restorePoints in CLearByCount");
        _countRestorePoints = countRestorePoints;
    }

    public List<RestorePointExtra> FindPointsToClear(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null)
            throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByCount algorithm (FindPointsToClean)");
        var restorePoints = extraBackupTask.RestorePointExtra.Take(_countRestorePoints).ToList();
        return restorePoints;
    }

    public void CleanPoints(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null)
            throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByCount algorithm (ClearPoints)");
        List<RestorePointExtra> restorePoints = FindPointsToClear(extraBackupTask);
        foreach (RestorePointExtra point in restorePoints)
        {
            extraBackupTask.DeleteRestorePoint(point);
        }
    }

    public ICleanSnapShot ToSnapshot() => new CleanByCountSnapShot()
    {
        CountRestorePoints = _countRestorePoints,
    };
}