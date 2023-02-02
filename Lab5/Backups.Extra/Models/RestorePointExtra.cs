using Backups.Extra.Tools;
using Backups.Models;
using Newtonsoft.Json;

namespace Backups.Extra.Models;

public class RestorePointExtra : IEquatable<RestorePointExtra>
{
    private readonly HashSet<BackupObject> _storedObjects;
    public RestorePointExtra(string name, DateTime creationDateTime, HashSet<BackupObject> storedObjects)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreationDateTime = creationDateTime;
        _storedObjects = storedObjects ?? throw new BackupsExtraException("Can't add this objects");
    }

    [JsonConstructor]
    private RestorePointExtra(string name, Guid id, DateTime creationDateTime, HashSet<BackupObject> storedObjects)
    {
        Name = name;
        Id = id;
        CreationDateTime = creationDateTime;
        _storedObjects = storedObjects ?? throw new BackupsExtraException("Can't add this objects");
    }

    public Guid Id { get; }
    public string Name { get; }
    public DateTime CreationDateTime { get; }
    public IReadOnlyCollection<BackupObject> StoredObjects => _storedObjects;
    public void Merge(RestorePointExtra point)
        => _storedObjects.UnionWith(point._storedObjects);

    public bool Equals(RestorePointExtra? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RestorePointExtra)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"{GetType().Name}(id = {Id})";
}