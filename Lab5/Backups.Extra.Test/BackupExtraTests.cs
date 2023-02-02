using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Entities.Repositories;
using Backups.Extra.Entities.StorageAlgorithmsExtra;
using Backups.Extra.Models;
using Backups.Extra.Models.Cleaner;
using Backups.Extra.Models.Logger;
using Backups.Extra.Models.MergeInstruction;
using Backups.Extra.Service;
using Backups.Models;
using Xunit;

namespace Backups.Extra.Test;

public class BackupExtraTests
{
    [Fact]
    public void AddTrackedObject_TrackedObjectAdded()
    {
        var service = new BackupServiceExtra();
        var arc = new BackupArchiverExtra(new SplitStorageExtra());
        var rep = new MockRepositoryExtra(new DirectoryInfo(@"../Directory"), arc);
        var settings = new BackupExtraSettings(new SoftMerge(), new CleanByCount(2));
        var log = new FileLogger(@"../Log", true);
        BackupTaskExtra backupTask = service.AddBackupTaskExtra("Test1SplitStorage", rep, settings, log);

        var firstFile = new BackupObject(new FileInfo(@"../FileA"));
        var secondFile = new BackupObject(new FileInfo(@"../FileB"));

        var jobObjects = new List<BackupObject>
        {
            firstFile,
            secondFile,
        };
        service.RunBackupTaskExtra(backupTask, jobObjects, "first");
        Assert.Equal(2, backupTask.BackupObjects.Count);
    }

    [Fact]
    public void AddTrackedObject_TrackedObjectAddedAddNewRestorePointAndMerge()
    {
        var service = new BackupServiceExtra();
        var arc = new BackupArchiverExtra(new SplitStorageExtra());
        var rep = new MockRepositoryExtra(new DirectoryInfo(@"../Directory"), arc);
        var settings = new BackupExtraSettings(new SoftMerge(), new CleanByCount(2));
        var log = new FileLogger(@"../Log", true);
        BackupTaskExtra backupTask = service.AddBackupTaskExtra("Test1SplitStorage", rep, settings, log);

        var firstFile = new BackupObject(new FileInfo(@"../FileA"));
        var secondFile = new BackupObject(new FileInfo(@"../FileB"));

        var jobObjects = new List<BackupObject>
        {
            firstFile,
            secondFile,
        };
        service.AddBackupObjectsToTrack(jobObjects, backupTask);
        RestorePointExtra firstPoint = backupTask.CreateAndSaveRestorePoint(DateTime.Now, "first");
        var thirdFile = new BackupObject(new FileInfo(@"../FileC"));
        service.AddBackupObjectToTrack(thirdFile, backupTask);
        RestorePointExtra secondPoint = backupTask.CreateAndSaveRestorePoint(DateTime.Now, "second");
        backupTask.MergePoints(firstPoint, secondPoint);
        Assert.Equal(1, backupTask.RestorePointExtra.Count);
    }

    [Fact]
    public void AddFiles_UseClearAlgorithms_CheckForPoints()
    {
        var service = new BackupServiceExtra();
        var arc = new BackupArchiverExtra(new SplitStorageExtra());
        var rep = new MockRepositoryExtra(new DirectoryInfo(@"../Directory"), arc);
        var compositeClearAlgo = new CleanByLimits(new List<IExtraCleanerAlgorithm>()
        {
            new CleanByCount(4),
            new CleanByDate(DateTime.Now.AddDays(-1)),
        });
        var settings = new BackupExtraSettings(new SoftMerge(), compositeClearAlgo);
        var log = new FileLogger(@"../Log", true);
        BackupTaskExtra backupTask = service.AddBackupTaskExtra("Test1SplitStorage", rep, settings, log);
        var firstFile = new BackupObject(new FileInfo(@"../FileA"));
        var secondFile = new BackupObject(new FileInfo(@"../FileB"));
        var thirdFile = new BackupObject(new FileInfo(@"../FileC"));
        var fourthFile = new BackupObject(new FileInfo(@"../FileD"));
        var jobObjects1 = new List<BackupObject>
        {
            firstFile,
            secondFile,
        };
        var jobObjects2 = new List<BackupObject>
        {
            thirdFile,
            fourthFile,
        };
        service.RunBackupTaskExtra(backupTask, jobObjects1, "first");
        service.RunBackupTaskExtra(backupTask, jobObjects2, "first");
        Assert.Equal(4, backupTask.RestorePointExtra.Last().StoredObjects.Count);
    }
}