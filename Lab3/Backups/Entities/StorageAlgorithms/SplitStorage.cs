using System.IO.Compression;
using System.Reflection;
using Backups.Models;

namespace Backups.Entities.StorageAlgorithms;

public class SplitStorage : IStorageAlgorithms
{
    private string _algorithmType = "Split";
    public byte[] GetZipBytes(ReadOnlySpan<byte> files, string archiveName)
    {
        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
        {
            ZipArchiveEntry entry = zipArchive.CreateEntry($"{archiveName}.zip", CompressionLevel.NoCompression);
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