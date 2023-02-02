using System.IO.Compression;
using Backups.Models;

namespace Backups.Entities.StorageAlgorithms;

public class SingleStorage : IStorageAlgorithms
{
    private string _algorithmType = "Single";
    public byte[] GetZipBytes(ReadOnlySpan<byte> files, string archiveName)
    {
        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
        {
            ZipArchiveEntry entry = zipArchive.CreateEntry($"{archiveName}.zip");
            using Stream entryStream = entry.Open();
            entryStream.Write(files);
        }

        return memoryStream.ToArray();
    }

    public string GetTypeOfAlgoritm()
    {
        return _algorithmType;
    }
}