using Backups.Models;

namespace Backups.Entities.StorageAlgorithms;

public interface IStorageAlgorithms
{
    byte[] GetZipBytes(ReadOnlySpan<byte> files, string archiveName);

    string GetTypeOfAlgoritm();
}