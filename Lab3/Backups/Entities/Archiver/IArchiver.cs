using Backups.Models;

namespace Backups.Entities.Archiver;

public interface IArchiver
{
    public byte[] Archive(ReadOnlySpan<byte> objects, string archiveName);
}