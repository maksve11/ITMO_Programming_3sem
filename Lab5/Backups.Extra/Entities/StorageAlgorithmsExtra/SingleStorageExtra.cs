using System.IO.Compression;
using Backups.Entities.StorageAlgorithms;
using Backups.Extra.Tools;

namespace Backups.Extra.Entities.StorageAlgorithmsExtra;

public class SingleStorageExtra : SingleStorage, IStorageAlgorithmsExtra
{
    public void ExtractFileFromZipData(byte[] objects, string fileName, string destination)
    {
        using var memoryStream = new MemoryStream(objects);
        using var externalArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        ZipArchiveEntry externalEntry = externalArchive.Entries.First();

        using Stream innerStream = externalEntry.Open();
        using var innerArchive = new ZipArchive(innerStream, ZipArchiveMode.Read);

        ZipArchiveEntry innerEntry = innerArchive.Entries.First(entry => entry.Name == fileName);
        innerEntry.ExtractToFile(destination, true);
    }

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
    {
        throw new BackupsExtraException("Can't merge files because it's single storage algorithm");
    }
}