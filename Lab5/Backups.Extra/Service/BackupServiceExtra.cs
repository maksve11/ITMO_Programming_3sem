using Backups.Entities;
using Backups.Entities.Archiver;
using Backups.Entities.Repositories;
using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Entities.BackupTaskExtra;
using Backups.Extra.Entities.Repositories;
using Backups.Extra.Models.Logger;
using Backups.Extra.Tools;
using Backups.Models;
using Backups.Tools;

namespace Backups.Extra.Service;

public class BackupServiceExtra : IBackupServiceExtra
{
    private readonly List<BackupTask> _tasks;
    private readonly List<BackupArchiver> _archivers;
    private readonly List<InMemoryRepository> _repositories;
    private readonly List<BackupTaskExtra> _tasksExtra;
    private readonly List<BackupArchiverExtra> _archiversExtra;
    private readonly List<InMemoryRepositoryExtra> _repositoriesMemory;
    private readonly List<MockRepositoryExtra> _repositoriesMock;
    private readonly List<BackupObject> _objects;

    public BackupServiceExtra()
    {
        _tasks = new List<BackupTask>();
        _archivers = new List<BackupArchiver>();
        _repositories = new List<InMemoryRepository>();
        _tasksExtra = new List<BackupTaskExtra>();
        _archiversExtra = new List<BackupArchiverExtra>();
        _repositoriesMemory = new List<InMemoryRepositoryExtra>();
        _objects = new List<BackupObject>();
        _repositoriesMock = new List<MockRepositoryExtra>();
    }

    public InMemoryRepository CreateRepository(DirectoryInfo workingDirectory, BackupArchiver zipArchiver)
    {
        var repository = new InMemoryRepository(workingDirectory, zipArchiver);
        if (_repositories.Contains(repository))
            throw new BackupsException("There's this repository yet");
        _repositories.Add(repository);
        return repository;
    }

    public BackupTask AddBackupTask(string name, InMemoryRepository repository)
    {
        var task = new BackupTask(name, repository);
        if (_tasks.Contains(task))
            throw new BackupsException("There's this task yet");
        _tasks.Add(task);
        return task;
    }

    public BackupTask AddBackupTask(string name, MockRepository repository)
    {
        var task = new BackupTask(name, repository);
        if (_tasks.Contains(task))
            throw new BackupsException("There's this task yet");
        _tasks.Add(task);
        return task;
    }

    public void RunBackupTask(BackupTask task, List<BackupObject> filesToBackup, string restorePointName)
    {
        if (!_tasks.Contains(task))
            throw new BackupsException("Can't run this task because there's no this task in list");
        if (filesToBackup.Count == 0)
            throw new BackupsException("Strange count of files to backup");
        task.AddBackupObjects(filesToBackup);
        task.CreateRestorePoint(DateTime.Now, restorePointName);
    }

    public void AddBackupObjects(List<BackupObject> objects)
    {
        if (objects.Count == 0)
            throw new BackupsException("Can't add this count of BackupObjects");
        foreach (BackupObject obj in objects)
        {
            AddBackupObject(obj);
        }
    }

    public void AddBackupObject(BackupObject obj)
    {
        if (string.IsNullOrWhiteSpace(obj.Path))
            throw new BackupsException("Can't add this obj");
        _objects.Add(obj);
    }

    public InMemoryRepositoryExtra CreateRepositoryExtra(DirectoryInfo workingDirectory, BackupArchiverExtra zipArchiver)
    {
        var repository = new InMemoryRepositoryExtra(workingDirectory, zipArchiver);
        if (_repositoriesMemory.Contains(repository))
            throw new BackupsException("There's this repository yet");
        _repositoriesMemory.Add(repository);
        return repository;
    }

    public BackupTaskExtra AddBackupTaskExtra(string name, InMemoryRepositoryExtra repository, BackupExtraSettings settings, ILogger logger)
    {
        var task = new BackupTaskExtra(name, repository, settings, logger);
        if (_tasksExtra.Contains(task))
            throw new BackupsException("There's this task yet");
        _tasksExtra.Add(task);
        return task;
    }

    public BackupTaskExtra AddBackupTaskExtra(string name, MockRepositoryExtra repository, BackupExtraSettings settings, ILogger logger)
    {
        var task = new BackupTaskExtra(name, repository, settings, logger);
        if (_tasksExtra.Contains(task))
            throw new BackupsException("There's this task yet");
        _tasksExtra.Add(task);
        return task;
    }

    public void RunBackupTaskExtra(BackupTaskExtra task, List<BackupObject> filesToBackup, string restorePointName)
    {
        if (!_tasksExtra.Contains(task))
            throw new BackupsException("Can't run this task because there's no this task in list");
        AddBackupObjectsToTrack(filesToBackup, task);
        task.CreateAndSaveRestorePoint(DateTime.Now, restorePointName);
    }

    public void AddBackupObjectsToTrack(List<BackupObject> objects, BackupTaskExtra task)
    {
        if (objects.Count == 0)
            throw new BackupsException("Strange count of files to backup");
        foreach (BackupObject file in objects)
        {
            task.AddObjectToTrack(file);
        }
    }

    public void AddBackupObjectToTrack(BackupObject obj, BackupTaskExtra task)
    {
        if (obj is null)
            throw new BackupsException("Strange count of files to backup");
        task.AddObjectToTrack(obj);
    }
}