using Backups.Entities.Archiver;

namespace Backups.Extra.Entities.ArchiverExtra;

public interface IArchiverExtra : IArchiver
{
    void ExtractFiles(byte[] objects, string fileName, string destination);
    byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData);
}