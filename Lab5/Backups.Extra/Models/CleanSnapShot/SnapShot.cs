using Backups.Extra.Entities;
using Backups.Extra.Entities.BackupTaskExtra;

namespace Backups.Extra.Models.CleanSnapShot;

public class SnapShot
{
    public SnapShot()
    {
        RepositoryPaths = new List<string>();
        ExtraBackupTasks = new List<BackupTaskExtra>();
    }

    public List<string> RepositoryPaths { get; set; }
    public List<BackupTaskExtra> ExtraBackupTasks { get; set; }
}