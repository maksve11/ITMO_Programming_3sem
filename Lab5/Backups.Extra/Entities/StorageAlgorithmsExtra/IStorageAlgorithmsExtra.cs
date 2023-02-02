using Backups.Entities.StorageAlgorithms;

namespace Backups.Extra.Entities.StorageAlgorithmsExtra;

public interface IStorageAlgorithmsExtra : IStorageAlgorithms
{
    public void ExtractFileFromZipData(byte[] objects, string fileName, string destination);

    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData);
}