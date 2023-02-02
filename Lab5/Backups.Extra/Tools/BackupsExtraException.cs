namespace Backups.Extra.Tools;

public class BackupsExtraException : Exception
{
    public BackupsExtraException()
    {
    }

    public BackupsExtraException(string message)
        : base(message)
    {
    }

    public BackupsExtraException(string message, Exception otherException)
        : base(message, otherException)
    {
    }
}