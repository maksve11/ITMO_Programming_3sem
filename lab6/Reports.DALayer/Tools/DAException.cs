namespace Reports.DALayer.Tools;

public class DaException : Exception
{
    public DaException()
    {
    }

    public DaException(string message)
        : base(message)
    {
    }

    public DaException(string message, Exception otherException)
        : base(message, otherException)
    {
    }
}