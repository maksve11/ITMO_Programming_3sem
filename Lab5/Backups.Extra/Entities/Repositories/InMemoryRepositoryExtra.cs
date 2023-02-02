using System.IO.Compression;
using Backups.Entities.Repositories;
using Backups.Extra.Entities.ArchiverExtra;
using Backups.Extra.Models;
using Backups.Extra.Tools;
using Backups.Models;

namespace Backups.Extra.Entities.Repositories;

public class InMemoryRepositoryExtra : IRepositoryExtra
{
    public InMemoryRepositoryExtra(DirectoryInfo workingDirectory, BackupArchiverExtra zipArchiver)
    {
        if (string.IsNullOrWhiteSpace(workingDirectory.FullName))
            throw new BackupsExtraException("Directory is invalid");

        ZipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));

        DirectoryInfo = workingDirectory;
        WorkingDirectory = DirectoryInfo;
        if (!WorkingDirectory.Exists)
            WorkingDirectory.Create();
    }

    public DirectoryInfo DirectoryInfo { get; }

    public DirectoryInfo WorkingDirectory { get; }
    public BackupArchiverExtra ZipArchiver { get; private set; }

    public FileInfo[] ReadAllFiles(string filePath)
    {
        return DirectoryInfo.GetFiles(filePath);
    }

    public byte[] GetBytes(IReadOnlyList<BackupObject> files)
    {
        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
        {
            foreach (BackupObject file in files)
            {
                byte[] data = File.ReadAllBytes(file.Path);
                ZipArchiveEntry entry = zipArchive.CreateEntry(file.Name, CompressionLevel.NoCompression);
                using Stream entryStream = entry.Open();
                entryStream.Write(data);
            }
        }

        return memoryStream.ToArray();
    }

    public byte[] GetBytes(BackupObject file)
    {
        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
        {
            byte[] data = File.ReadAllBytes(file.Path);
            ZipArchiveEntry entry = zipArchive.CreateEntry(file.Name, CompressionLevel.NoCompression);
            using Stream entryStream = entry.Open();
            entryStream.Write(data);
        }

        return memoryStream.ToArray();
    }

    public string Write(string backupTaskName, string restorePointName, IReadOnlyList<BackupObject> objects)
    {
        string path = Path.Combine(
            WorkingDirectory.FullName,
            backupTaskName,
            restorePointName);

        List<BackupObject> files = GetFilesFromBackupObjects(objects);

        if (ZipArchiver.StorageAlgorithm.GetTypeOfAlgoritm() == "Split")
        {
            foreach (BackupObject obj in objects)
            {
                byte[] zipFiles = GetBytes(obj);
                byte[] zipBytes = ZipArchiver.Archive(zipFiles, obj.Name.Replace('.', '_'));
                ExtractZipToDirectory(zipBytes, path);
            }
        }
        else
        {
            byte[] zipFiles = GetBytes(files);
            byte[] zipBytes = ZipArchiver.Archive(zipFiles, restorePointName);
            ExtractZipToDirectory(zipBytes, path);
        }

        return path;
    }

    public byte[] GetZipData(string backupObjId, string restorePointId)
    {
        string path = GetPathToData(backupObjId, restorePointId);

        if (File.Exists(path))
        {
            throw new BackupsExtraException(
                $"file for restore point with id {restorePointId} " +
                $"of backup job with id {backupObjId} not found");
        }

        return File.ReadAllBytes(path);
    }

    public void SaveZipData(string backupObjId, string restorePointId, byte[] zipData)
    {
        string location = GetDataLocation(backupObjId);
        if (!Directory.Exists(location))
            Directory.CreateDirectory(location);

        string path = GetPathToData(backupObjId, restorePointId);

        File.WriteAllBytes(path, zipData);
    }

    public void DeleteRestorePoint(string backupObjId, string restorePointId)
    {
        string path = GetPathToData(backupObjId, restorePointId);

        if (File.Exists(path))
        {
            throw new BackupsExtraException(
                $"file for restore point with id {restorePointId} " +
                $"of backup task with id {backupObjId} not found");
        }

        File.Delete(path);
    }

    public void ExtractFiles(byte[] objects, string fileName, string destination)
    {
        ZipArchiver.ExtractFiles(objects, fileName, destination);
    }

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
    {
        return ZipArchiver.StorageAlgorithm.MergeZipEntries(baseZipData, mergedZipData);
    }

    private static List<BackupObject> GetFilesFromBackupObjects(IEnumerable<BackupObject> objects)
        => objects.Select(obj => obj).ToList();

    private static void ExtractZipToDirectory(byte[] zipBytes, string path)
    {
        var memoryStream = new MemoryStream(zipBytes);
        var zipArchive = new ZipArchive(memoryStream);
        zipArchive.ExtractToDirectory(path);
    }

    private string GetDataLocation(string backupTaskId)
        => Path.Combine(DirectoryInfo.FullName, backupTaskId);

    private string GetPathToData(string backupTaskId, string restorePointId)
        => Path.Combine(DirectoryInfo.FullName, backupTaskId, restorePointId);
}