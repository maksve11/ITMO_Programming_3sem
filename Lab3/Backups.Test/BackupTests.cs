using Backups.Entities;
using Backups.Entities.Archiver;
using Backups.Entities.Repositories;
using Backups.Entities.StorageAlgorithms;
using Backups.Models;
using Backups.Service;
using Moq;
using Xunit;
using MockRepository = Backups.Entities.Repositories.MockRepository;

namespace Backups.Test;

public class BackupTests
{
    [Fact]
    public void CreateBackupJobAddTwoFilesDeleteOne_InBackupTwoRestorePointsThreeStorages()
    {
        var service = new BackupService();
        var arc = new BackupArchiver(new SplitStorage());
        var rep = new MockRepository(new DirectoryInfo("../Directory"), arc);
        BackupTask backupTask = service.AddBackupTask("Test1SplitStorage", rep);

        var firstFile = new BackupObject(new FileInfo("../Test/FileA"));
        var secondFile = new BackupObject(new FileInfo("../Test/FileB"));

        var jobObjects = new List<BackupObject>
        {
            firstFile,
            secondFile,
        };

        service.AddBackupObjects(jobObjects);
        service.RunBackupTask(backupTask, jobObjects, "first");

        backupTask.RemoveBackupObject(firstFile);
        backupTask.CreateRestorePoint(DateTime.Now, "second");
        int storagesCount = backupTask.RestorePoints.Sum(restorePoint => restorePoint.Storages.Count);
        int restorePointCount = backupTask.RestorePoints.Count;
        Assert.Equal(2, restorePointCount);
        Assert.Equal(3, storagesCount);
    }

    [Fact]
    public void CreateBackupJobAddTwoFiles_CheckThatFilesAndDirectoriesWereCreated()
    {
        var service = new BackupService();
        var arc = new BackupArchiver(new SplitStorage());
        var rep = new MockRepository(new DirectoryInfo("../Directory"), arc);
        BackupTask backupTask = service.AddBackupTask("Test2SingleStorage", rep);

        var firstFile = new BackupObject(new FileInfo("../Test/FileA"));
        var secondFile = new BackupObject(new FileInfo("../Test/FileB"));

        var jobObjects = new List<BackupObject>
        {
            firstFile,
            secondFile,
        };

        service.AddBackupObjects(jobObjects);
        service.RunBackupTask(backupTask, jobObjects, "first");
        Assert.Equal(1, backupTask.RestorePoints.Count);
    }
}