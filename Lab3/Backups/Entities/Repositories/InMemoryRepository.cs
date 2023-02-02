using System.IO.Compression;
using Backups.Entities.Archiver;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities.Repositories;

public class InMemoryRepository : IRepository
{
    private readonly BackupArchiver _zipArchiver;

    public InMemoryRepository(DirectoryInfo workingDirectory, BackupArchiver zipArchiver)
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
        return Directory.GetFiles(filePath);
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

        if (_zipArchiver.StorageAlgorithm.GetTypeOfAlgoritm() == "Split")
        {
            foreach (BackupObject obj in objects)
            {
                byte[] zipFiles = GetBytes(obj);
                byte[] zipBytes = _zipArchiver.Archive(zipFiles, obj.Name.Replace('.', '_'));
                ExtractZipToDirectory(zipBytes, path);
            }
        }
        else
        {
            byte[] zipFiles = GetBytes(files);
            byte[] zipBytes = _zipArchiver.Archive(zipFiles, restorePointName);
            ExtractZipToDirectory(zipBytes, path);
        }

        return path;
    }

    private static List<BackupObject> GetFilesFromBackupObjects(IEnumerable<BackupObject> objects)
        => objects.Select(obj => obj).ToList();

    private static void ExtractZipToDirectory(byte[] zipBytes, string path)
    {
        var memoryStream = new MemoryStream(zipBytes);
        var zipArchive = new ZipArchive(memoryStream);
        zipArchive.ExtractToDirectory(path);
    }
}