using Backups.Entities;
using Backups.Entities.Archiver;
using Backups.Entities.Repositories;
using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Entities.Repositories;
using Backups.Extra.Models.Logger;
using Backups.Models;
using Backups.Service;

namespace Backups.Extra.Service;

public interface IBackupServiceExtra : IBackupService
{
    InMemoryRepositoryExtra CreateRepositoryExtra(DirectoryInfo workingDirectory, BackupArchiverExtra zipArchiver);
    BackupTaskExtra AddBackupTaskExtra(string name, InMemoryRepositoryExtra repository, BackupExtraSettings settings, ILogger logger);
    BackupTaskExtra AddBackupTaskExtra(string name, MockRepositoryExtra repository, BackupExtraSettings settings, ILogger logger);
    void RunBackupTaskExtra(BackupTaskExtra task, List<BackupObject> filesToBackup, string restorePointName);
    void AddBackupObjectsToTrack(List<BackupObject> objects, BackupTaskExtra task);
    void AddBackupObjectToTrack(BackupObject obj, BackupTaskExtra task);
}