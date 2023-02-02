using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Models.MergeInstruction;

public class SoftMerge : IMergeInstruction
{
    public void AddPoint(BackupTaskExtra extraBackupTask, List<BackupObject> files)
    {
        foreach (BackupObject file in files)
        {
            if (string.IsNullOrWhiteSpace(file.Path))
                throw new BackupsExtraException("Can't add this file");
            extraBackupTask.AddObjectToTrack(file);
        }

        RestorePointExtra newPoint = extraBackupTask.RestorePointExtra.LastOrDefault() ?? throw new BackupsExtraException("There's no any Restore Points");
        var pointsToMerge = extraBackupTask.GetAlgorithm().FindPointsToClear(extraBackupTask).ToList();
        foreach (RestorePointExtra point in pointsToMerge.Where(point => !point.Equals(newPoint)))
        {
            extraBackupTask.MergePoints(point, newPoint);
        }
    }
}