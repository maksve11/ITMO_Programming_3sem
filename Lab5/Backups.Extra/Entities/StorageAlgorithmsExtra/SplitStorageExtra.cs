using System.IO.Compression;
using Backups.Entities.StorageAlgorithms;

namespace Backups.Extra.Entities.StorageAlgorithmsExtra;

public class SplitStorageExtra : SplitStorage, IStorageAlgorithmsExtra
{
    public void ExtractFileFromZipData(byte[] objects, string fileName, string destination)
    {
        using var memoryStream = new MemoryStream(objects);
        using var externalArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        ZipArchiveEntry externalEntry = externalArchive.Entries.First(entry => entry.Name == fileName);

        using Stream innerStream = externalEntry.Open();
        using var innerArchive = new ZipArchive(innerStream, ZipArchiveMode.Read);

        ZipArchiveEntry innerEntry = innerArchive.Entries.First();
        innerEntry.ExtractToFile(destination, true);
    }

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
    {
        using var externalMemoryStream = new MemoryStream();
        externalMemoryStream.Write(baseZipData);
        externalMemoryStream.Seek(0, SeekOrigin.Begin);

        var externalArchive = new ZipArchive(externalMemoryStream, ZipArchiveMode.Update, false);

        using (externalArchive)
        {
            using var mergedExternalMemoryStream = new MemoryStream(mergedZipData);
            using var mergedExternalArchive = new ZipArchive(mergedExternalMemoryStream, ZipArchiveMode.Read);

            var externalEntryNames = externalArchive.Entries.Select(entry => entry.Name).ToHashSet();

            foreach (ZipArchiveEntry mergedExternalEntry in mergedExternalArchive.Entries)
            {
                if (externalEntryNames.Contains(mergedExternalEntry.Name))
                    continue;

                using Stream mergedEntryStream = mergedExternalEntry.Open();
                ZipArchiveEntry newExternalEntry = externalArchive.CreateEntry(mergedExternalEntry.Name);
                using Stream newEntryStream = newExternalEntry.Open();

                mergedEntryStream.CopyTo(newEntryStream);
            }
        }

        return externalMemoryStream.ToArray();
    }
}