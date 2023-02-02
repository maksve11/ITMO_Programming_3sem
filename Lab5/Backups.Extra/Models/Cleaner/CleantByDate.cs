using System.Collections.Immutable;
using Backups.Extra.Entities;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Models.CleanSnapShot;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public class CleanByDate : IExtraCleanerAlgorithm
{
    private readonly DateTime _timeUntilWeStoreFiles;

    public CleanByDate(DateTime timeUntilWeStoreFiles)
    {
        _timeUntilWeStoreFiles = timeUntilWeStoreFiles;
    }

    public List<RestorePointExtra> FindPointsToClear(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null) throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByDate algorithm (FindPointsToClean)");
        var restorePoints = extraBackupTask.RestorePointExtra.Where(restorePoint =>
            restorePoint.CreationDateTime <= _timeUntilWeStoreFiles).ToList();
        return restorePoints;
    }

    public void CleanPoints(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null) throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByDate algorithm (ClearPoints)");
        List<RestorePointExtra> restorePoints = FindPointsToClear(extraBackupTask);
        foreach (RestorePointExtra point in restorePoints)
        {
            extraBackupTask.DeleteRestorePoint(point);
        }
    }

    public ICleanSnapShot ToSnapshot() => new CleanByDateSnapShot()
    {
        TimeUntilWeStoreFiles = _timeUntilWeStoreFiles,
    };
}