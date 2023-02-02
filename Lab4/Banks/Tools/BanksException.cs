namespace Banks.Tools;

public class BanksException : Exception
{
    public BanksException()
    {
    }

    public BanksException(string message)
        : base(message)
    {
    }

    public BanksException(string message, Exception otherException)
        : base(message, otherException)
    {
    }
}