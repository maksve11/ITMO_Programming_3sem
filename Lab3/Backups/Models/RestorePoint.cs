using Backups.Entities.StorageAlgorithms;
using Backups.Tools;

namespace Backups.Models;

public class RestorePoint
{
    private readonly List<Storage> _storages;
    public RestorePoint(string name, string path, List<Storage> objects, DateTime creationTime)
    {
        Time = creationTime;
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Can't create this restore point with this parameters");
        Name = name;
        Path = path;
        if (objects.Count == 0)
            throw new BackupsException("Can't create restore point with this count of storages");
        _storages = objects ?? throw new ArgumentNullException(nameof(objects));
    }

    public DateTime Time { get; }
    public string Name { get; }
    public string Path { get; }

    public IReadOnlyList<Storage> Storages => _storages.AsReadOnly();

    public void AddStorage(Storage storage)
    {
        if (_storages.Contains(storage))
            throw new BackupsException("There's this storage yet");
        _storages.Add(storage);
    }
}