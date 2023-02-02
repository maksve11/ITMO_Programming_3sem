using System.Collections.Immutable;
using Backups.Extra.Entities;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Models.CleanSnapShot;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public class CleanByLimits : IExtraCleanerAlgorithm
{
    private readonly List<IExtraCleanerAlgorithm> _extraAlgorithms;

    public CleanByLimits(List<IExtraCleanerAlgorithm> extraAlgorithms)
    {
        _extraAlgorithms = extraAlgorithms ??
                           throw new BackupsExtraException("Invalid extraAlgorithms in ClearByLimits algorithm");
    }

    public List<RestorePointExtra> FindPointsToClear(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null)
            throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByLimits algorithm (FindPointsToClean)");
        return _extraAlgorithms.SelectMany(algorithm => algorithm.FindPointsToClear(extraBackupTask)).ToList();
    }

    public void CleanPoints(BackupTaskExtra extraBackupTask)
    {
        if (extraBackupTask is null)
            throw new BackupsExtraException("Invalid ExtraBackupTask in ClearByDate algorithm (ClearPoints)");
        List<RestorePointExtra> restorePoints = FindPointsToClear(extraBackupTask);
        foreach (RestorePointExtra point in restorePoints)
        {
            extraBackupTask.DeleteRestorePoint(point);
        }
    }

    public ICleanSnapShot ToSnapshot() => new CleanByLimitsSnapShot()
    {
        Algorithms = _extraAlgorithms.Select(x => x.ToSnapshot()).ToList(),
    };
}