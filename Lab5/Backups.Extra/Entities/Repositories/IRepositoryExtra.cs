using Backups.Entities.Repositories;

namespace Backups.Extra.Entities.Repositories;

public interface IRepositoryExtra : IRepository
{
    byte[] GetZipData(string backupObjId, string restorePointId);
    void SaveZipData(string backupObjId, string restorePointId, byte[] zipData);
    void DeleteRestorePoint(string backupObjId, string restorePointId);
    void ExtractFiles(byte[] objects, string fileName, string destination);
    public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData);
}