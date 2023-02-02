using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Models;

namespace Backups.Extra.Models.MergeInstruction;

public interface IMergeInstruction
{
    void AddPoint(BackupTaskExtra extraBackupTask, List<BackupObject> files);
}