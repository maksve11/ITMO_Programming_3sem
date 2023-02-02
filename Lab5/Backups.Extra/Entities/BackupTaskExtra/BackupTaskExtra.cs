using Backups.Entities;
using Backups.Entities.Repositories;
using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Entities.Repositories;
using Backups.Extra.Models;
using Backups.Extra.Models.Cleaner;
using Backups.Extra.Models.Logger;
using Backups.Extra.Models.MergeInstruction;
using Backups.Extra.Tools;
using Backups.Models;
using Backups.Tools;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace Backups.Extra.Entities.BackupTaskExtra;

public class BackupTaskExtra
{
    private readonly BackupExtraSettings _settings;
    private readonly List<BackupObject> _backupObjects;
    private readonly List<RestorePointExtra> _restorePoints;
    private readonly IRepositoryExtra _backupRepository;
    public BackupTaskExtra(string name, IRepositoryExtra backupRepository, BackupExtraSettings settings, ILogger logger)
    {
        _settings = settings ?? throw new BackupsExtraException("Extra Backup settings is null");
        Id = Guid.NewGuid();
        Logger = logger;
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsExtraException("Can't be BackupTask with this name");
        Name = name;
        _backupObjects = new List<BackupObject>();
        _restorePoints = new List<RestorePointExtra>();
        _backupRepository = backupRepository;
    }

    [JsonConstructor]
    private BackupTaskExtra(Guid id, string name, IRepositoryExtra backupRepository, BackupExtraSettings settings, ILogger logger)
    {
        _settings = settings ?? throw new BackupsExtraException("Extra Backup settings is null");
        Id = id;
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsExtraException("Can't be BackupTask with this name");
        Name = name;
        Logger = logger;
        _backupObjects = new List<BackupObject>();
        _restorePoints = new List<RestorePointExtra>();
        _backupRepository = backupRepository;
    }

    public Guid Id { get; }
    public ILogger Logger { get; }
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects.AsReadOnly();
    public IReadOnlyList<RestorePointExtra> RestorePointExtra => _restorePoints.AsReadOnly();
    public string Name { get; }

    public IExtraCleanerAlgorithm GetAlgorithm() => _settings.ExtraAlgorithm();

    public IMergeInstruction GetInstruction() => _settings.MergeInstruction();

    public void AddObjectToTrack(BackupObject newObject)
    {
        if (newObject == null)
            throw new BackupsExtraException("Can't add this Backup Object");

        if (BackupObjects.Contains(newObject))
        {
            string message = $"Backup object {newObject} already tracked by {this}";
            Logger?.LogInformation(this.ToString(), message);
            throw new BackupsExtraException(message);
        }
        else
        {
            string message = $"Backup object {newObject} successful added  {this}";
            _backupObjects.Add(newObject);
            Logger?.LogInformation(this.ToString(), message);
        }

        Logger?.LogInformation(this.ToString(), $"{newObject} successfully added to {this}");
    }

    public RestorePointExtra CreateAndSaveRestorePoint(DateTime creationDateTime, string restorePointId)
    {
        if (BackupObjects.Count == 0)
        {
            string message = $"Attempt to create restore point without files in {this}";
            Logger?.LogInformation(this.ToString(), message);
            throw new BackupsExtraException(message);
        }

        Logger?.LogInformation(this.ToString(), "Creating new restore point...");

        var objectsShot = BackupObjects.ToHashSet();
        var point = new RestorePointExtra(restorePointId, creationDateTime, objectsShot);

        string path = _backupRepository.Write(Name, restorePointId, objectsShot.ToList());
        var storages = new List<Storage>();
        foreach (BackupObject obj in _backupObjects)
        {
            var storage = new Storage(obj);
            storages.Add(storage);
        }

        _restorePoints.Add(point);

        Logger?.LogInformation(this.ToString(), $"{point} successfully created");
        return point;
    }

    public void DeleteRestorePoint(RestorePointExtra restorePointToDelete)
    {
        if (restorePointToDelete is null) throw new BackupsExtraException("Invalid restorePoint in DeleteRestorePoint");
        _restorePoints.Remove(restorePointToDelete);
        _backupRepository.DeleteRestorePoint(Name, restorePointToDelete.Name);
        Logger?.LogInformation(this.ToString(), $"{restorePointToDelete} successfully deleted");
    }

    public void DeleteAllRestorePoints()
    {
        var restorePointsToDelete = _settings.ExtraAlgorithm().FindPointsToClear(this).ToList();
        foreach (RestorePointExtra? restorePoint in restorePointsToDelete)
        {
            if (restorePoint is null) throw new BackupsExtraException("Invalid restorePoint");
            _restorePoints.Remove(restorePoint);
        }

        Logger?.LogInformation(this.ToString(), $"all restore points successfully deleted");
    }

    public void RestoreFilesFromRestorePoint(RestorePointExtra restorePoint, DirectoryInfo? destination)
    {
        Logger?.LogInformation(this.ToString(), $"Restoring files from {restorePoint}");

        byte[] zipData = _backupRepository.GetZipData(Name, restorePoint.Name);
        foreach (BackupObject jobObject in restorePoint.StoredObjects)
        {
            string fileName = jobObject.FullInfo.Name;
            destination ??= jobObject.FullInfo.Directory;
            string path = Path.Combine(destination?.ToString() ?? string.Empty, fileName);
            _backupRepository.ExtractFiles(zipData, fileName, path);
        }

        Logger?.LogInformation(this.ToString(), $"Files from {restorePoint} successfully restored");
    }

    public void MergePoints(RestorePointExtra mergedPoint, RestorePointExtra to)
    {
        byte[] mergedPointZipData = _backupRepository.GetZipData(Name, mergedPoint.Name);
        byte[] toPointZipData = _backupRepository.GetZipData(Name, to.Name);
        toPointZipData = _backupRepository.MergeZipEntries(toPointZipData, mergedPointZipData);
        _backupRepository.SaveZipData(Name, to.Name, toPointZipData);
        to.Merge(mergedPoint);
        DeleteRestorePoint(mergedPoint);
        Logger.LogInformation(this.ToString(), $"{mergedPoint} and {to} was merged");
    }

    public override string ToString() => $"{GetType().Name}(id = {Id})";

    private static List<FileInfo> ExtractFileInfos(IEnumerable<BackupObject> jobObjects)
        => jobObjects.Select(obj => obj.FullInfo).ToList();
}