namespace Banks.Entities.MementoPattern;

public class Memento : IMemento
{
    private readonly decimal _balanceStatus;

    public Memento(decimal balanceStatus)
    {
        _balanceStatus = balanceStatus;
    }

    public decimal GetStatus()
    {
        return _balanceStatus;
    }
}