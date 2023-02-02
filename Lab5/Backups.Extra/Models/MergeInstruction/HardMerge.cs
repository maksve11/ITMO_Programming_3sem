using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Models.MergeInstruction;

public class HardMerge : IMergeInstruction
{
    public void AddPoint(BackupTaskExtra extraBackupTask, List<BackupObject> files)
    {
        foreach (BackupObject file in files)
        {
            if (string.IsNullOrWhiteSpace(file.Path))
                throw new BackupsExtraException("Can't add this file");
            extraBackupTask.AddObjectToTrack(file);
        }

        extraBackupTask.DeleteAllRestorePoints();
    }
}