using Backups.Entities;
using Backups.Entities.Archiver;
using Backups.Entities.Repositories;
using Backups.Entities.StorageAlgorithms;
using Backups.Models;

namespace Backups.Service;

public interface IBackupService
{
    InMemoryRepository CreateRepository(DirectoryInfo workingDirectory, BackupArchiver zipArchiver);
    BackupTask AddBackupTask(string name, InMemoryRepository repository);
    BackupTask AddBackupTask(string name, MockRepository repository);
    void RunBackupTask(BackupTask task, List<BackupObject> filesToBackup, string restorePointName);
    void AddBackupObjects(List<BackupObject> objects);
    void AddBackupObject(BackupObject obj);
}