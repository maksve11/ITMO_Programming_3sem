namespace Reports.BiLayer.Tools;

public class BiException : Exception
{
    public BiException()
    {
    }

    public BiException(string message)
        : base(message)
    {
    }

    public BiException(string message, Exception otherException)
        : base(message, otherException)
    {
    }
}