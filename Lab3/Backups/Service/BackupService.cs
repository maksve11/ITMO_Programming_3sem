using Backups.Entities;
using Backups.Entities.Archiver;
using Backups.Entities.Repositories;
using Backups.Models;
using Backups.Tools;

namespace Backups.Service;

public class BackupService : IBackupService
{
    private readonly List<BackupTask> _tasks;
    private readonly List<BackupArchiver> _archivers;
    private readonly List<InMemoryRepository> _repositories;
    private readonly List<BackupObject> _objects;

    public BackupService()
    {
        _tasks = new List<BackupTask>();
        _archivers = new List<BackupArchiver>();
        _repositories = new List<InMemoryRepository>();
        _objects = new List<BackupObject>();
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
}