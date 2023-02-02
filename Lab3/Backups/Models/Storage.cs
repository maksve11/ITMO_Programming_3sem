using System.IO.Compression;
using Backups.Tools;

namespace Backups.Models;

public class Storage
{
    public Storage(BackupObject file)
    {
        if (string.IsNullOrWhiteSpace(file.Path))
            throw new BackupsException("Can't create Storage with this file");
        ObjOfBackup = file;
    }

    public BackupObject ObjOfBackup { get; private set; }

    public void ChangeFileInStorage(BackupObject newFile)
    {
        if (ObjOfBackup.Equals(newFile))
            throw new BackupsException("Can't change File in this Storage");
        ObjOfBackup = newFile;
    }
}