using Backups.Tools;

namespace Backups.Models;

public class BackupObject : IEquatable<BackupObject>
{
    public BackupObject(FileInfo file)
    {
        if (string.IsNullOrWhiteSpace(file.DirectoryName))
            throw new BackupsException("Directory name of file is empty");
        FullInfo = file;
    }

    public FileInfo FullInfo { get; }

    public string Path => FullInfo.FullName;
    public string Name => FullInfo.Name;

    public bool Equals(BackupObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FullInfo.Equals(other.FullInfo);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BackupObject)obj);
    }

    public override int GetHashCode()
    {
        return FullInfo.GetHashCode();
    }
}