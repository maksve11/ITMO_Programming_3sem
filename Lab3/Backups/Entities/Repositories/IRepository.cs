using Backups.Models;

namespace Backups.Entities.Repositories;

public interface IRepository
{
    FileInfo[] ReadAllFiles(string filePath);
    byte[] GetBytes(IReadOnlyList<BackupObject> files);
    byte[] GetBytes(BackupObject file);
    public string Write(string backupTaskName, string restorePointName, IReadOnlyList<BackupObject> objects);
}