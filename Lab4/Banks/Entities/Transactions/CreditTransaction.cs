using Banks.Entities.Accounts;
using Banks.Entities.MementoPattern;
using Banks.Tools;

namespace Banks.Entities.Transactions;

public class CreditTransaction : Transaction
{
    public CreditTransaction(Account account)
        : base(account)
    {
    }

    public override void Withdraw(decimal money)
    {
        if (Account is CreditAccount account)
        {
            if (money <= 0 || account.Balance < money) throw new BanksException("Money cannot be null or negative");
            if (!account.IsWithdrawPossible(money)) throw new BanksException("Withdrawal is not available");

            account.CareTaker.SaveState(new Memento(account.Balance));
            account.Balance -= money;
        }
        else
        {
            TransactionCell?.Withdraw(money);
        }
    }

    public override void Transfer(Account replenishAccount, decimal money)
    {
        if (Account is CreditAccount account)
        {
            if (money <= 0) throw new BanksException("Money can't be null or negative");
            if (!account.IsWithdrawPossible(money)) throw new BanksException("Withdrawal is not possible");

            account.CareTaker.SaveState(new Memento(account.Balance));
            replenishAccount.CareTaker.SaveState(new Memento(replenishAccount.Balance));

            account.Balance -= money;
            replenishAccount.Balance += money;
        }
        else
        {
            TransactionCell?.Transfer(replenishAccount, money);
        }
    }
}