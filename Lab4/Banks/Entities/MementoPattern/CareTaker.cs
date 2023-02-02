using Banks.Entities.Accounts;
using Banks.Tools;

namespace Banks.Entities.MementoPattern;

public class CareTaker
{
    private readonly List<IMemento> _balanceStates = new List<IMemento>();
    private readonly Account _account;

    public CareTaker(Account account)
    {
        _account = account;
    }

    public IReadOnlyList<IMemento> BalanceStates => _balanceStates;

    public void SaveState(Memento balance)
    {
        _balanceStates.Add(balance);
    }

    public IMemento Restore()
    {
        IMemento result = _balanceStates.LastOrDefault() ??
                     throw new BanksException("Cannot restore the state. Collection is empty :(");
        _account.Restore(result);
        _balanceStates.Remove(result);
        return result;
    }
}