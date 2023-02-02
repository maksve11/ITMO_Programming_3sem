using Backups.Entities.Archiver;
using Backups.Entities.StorageAlgorithms;
using Backups.Extra.Entities.StorageAlgorithmsExtra;

namespace Backups.Extra.Entities.ArchiverExtra;

public class BackupArchiverExtra : IArchiverExtra
{
    public BackupArchiverExtra(IStorageAlgorithmsExtra storageAlgorithm)
    {
        StorageAlgorithm = storageAlgorithm;
    }

    public IStorageAlgorithmsExtra StorageAlgorithm { get; }

    public byte[] Archive(ReadOnlySpan<byte> objects, string archiveName)
    {
        return StorageAlgorithm.GetZipBytes(objects, archiveName);
    }

    public void ExtractFiles(byte[] objects, string fileName, string destination)
    {
        StorageAlgorithm.ExtractFileFromZipData(objects, fileName, destination);
    }

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
    {
        return StorageAlgorithm.MergeZipEntries(baseZipData, mergedZipData);
    }
}