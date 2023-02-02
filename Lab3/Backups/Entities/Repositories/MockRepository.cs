using Backups.Entities.Archiver;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities.Repositories;

public class MockRepository : IRepository
{
    private readonly BackupArchiver _zipArchiver;

    public MockRepository(DirectoryInfo workingDirectory, BackupArchiver zipArchiver)
    {
        if (string.IsNullOrWhiteSpace(workingDirectory.FullName))
            throw new BackupsException("Directory is invalid");

        _zipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));

        Directory = workingDirectory;
        WorkingDirectory = Directory;
        if (!WorkingDirectory.Exists)
            WorkingDirectory.Create();
    }

    public DirectoryInfo Directory { get; }

    public DirectoryInfo WorkingDirectory { get; }

    public FileInfo[] ReadAllFiles(string filePath)
    {
        var test = new FileInfo[] { new FileInfo("D/BackupsTest") };
        return test;
    }

    public byte[] GetBytes(IReadOnlyList<BackupObject> files)
    {
        return new byte[] { 1, 0, 1, 1 };
    }

    public byte[] GetBytes(BackupObject file)
    {
        return new byte[] { 1, 0, 1, 1 };
    }

    public string Write(string backupTaskName, string restorePointName, IReadOnlyList<BackupObject> objects)
    {
        return "D/Backups";
    }
}