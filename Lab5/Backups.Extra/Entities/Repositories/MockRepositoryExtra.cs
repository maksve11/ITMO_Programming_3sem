using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Entities.Repositories;

public class MockRepositoryExtra : IRepositoryExtra
{
    private readonly BackupArchiverExtra _zipArchiver;
    public MockRepositoryExtra(DirectoryInfo workingDirectory, BackupArchiverExtra zipArchiver)
    {
        if (string.IsNullOrWhiteSpace(workingDirectory.FullName))
            throw new BackupsExtraException("Directory is invalid");

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
        return "D/BackupsTest";
    }

    public byte[] GetZipData(string backupObjId, string restorePointId)
    {
        return new byte[] { 1, 0, 1, 1 };
    }

    public void SaveZipData(string backupObjId, string restorePointId, byte[] zipData)
    {
        return;
    }

    public void DeleteRestorePoint(string backupObjId, string restorePointId)
    {
        return;
    }

    public void ExtractFiles(byte[] objects, string fileName, string destination)
    {
        return;
    }

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
    {
        return new byte[] { 1, 0, 1, 1 };
    }
}