using Backups.Entities.Repositories;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities;

public class BackupTask : IEquatable<BackupTask>
{
    private readonly List<BackupObject> _backupObjects;
    private readonly List<RestorePoint> _restorePoints;
    private readonly IRepository _backupRepository;

    public BackupTask(string name, IRepository backupRepository)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        _backupObjects = new List<BackupObject>();
        _restorePoints = new List<RestorePoint>();
    }

    public string Name { get; }
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects.AsReadOnly();
    public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

    public void AddBackupObject(BackupObject obj)
    {
        if (_backupObjects.Contains(obj))
            throw new BackupsException("This Object there's in this BackupTask yet");
        _backupObjects.Add(obj);
    }

    public void AddBackupObjects(List<BackupObject> objects)
    {
        foreach (BackupObject obj in objects)
        {
            AddBackupObject(obj);
        }
    }

    public void RemoveBackupObject(BackupObject obj)
    {
        if (!_backupObjects.Contains(obj))
            throw new BackupsException("This Object there's not in this BackupTask");
        _backupObjects.Remove(obj);
    }

    public void CreateRestorePoint(DateTime creationTime, string restorePointId)
    {
        if (HasRestorePointWithName(restorePointId))
            throw new BackupsException("Restore point with id already exists");

        string path = _backupRepository.Write(Name, restorePointId, _backupObjects);
        var storages = new List<Storage>();
        foreach (BackupObject obj in _backupObjects)
        {
            var storage = new Storage(obj);
            storages.Add(storage);
        }

        var restorePoint = new RestorePoint(restorePointId, path, storages, creationTime);
        _restorePoints.Add(restorePoint);
    }

    public bool Equals(BackupTask? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name.Equals(other.Name);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BackupTask)obj);
    }

    public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

    private bool HasRestorePointWithName(string restorePointName) => _restorePoints.Any(restorePoint => restorePoint.Name.Equals(restorePointName));
}