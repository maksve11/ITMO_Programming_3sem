using Backups.Entities.StorageAlgorithms;

namespace Backups.Entities.Archiver;

public class BackupArchiver : IArchiver
{
    public BackupArchiver(IStorageAlgorithms storageAlgorithm)
    {
        StorageAlgorithm = storageAlgorithm;
    }

    public IStorageAlgorithms StorageAlgorithm { get; }
    public byte[] Archive(ReadOnlySpan<byte> objects, string archiveName)
    {
        return StorageAlgorithm.GetZipBytes(objects, archiveName);
    }
}