using Banks.Entities.Accounts;
using Banks.Entities.MementoPattern;
using Banks.Tools;

namespace Banks.Entities.Transactions;

public class ReplenishTransaction : Transaction
{
    public ReplenishTransaction(Account account)
        : base(account)
    {
    }

    public override void Replenish(decimal money)
    {
        if (money <= 0) throw new BanksException("Money can't be null or negative");
        Account.CareTaker.SaveState(new Memento(Account.Balance));

        Account.Balance += money;
    }
}