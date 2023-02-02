using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions;

public class Transaction : ITransactionCell
{
    public Transaction(Account account)
    {
        Account = account;
    }

    protected ITransactionCell? TransactionCell { get; private set; }
    protected Account Account { get; }

    public ITransactionCell? SetNext(ITransactionCell? cell)
    {
        TransactionCell = cell;
        return cell;
    }

    public virtual void Replenish(decimal money)
    {
        TransactionCell = new ReplenishTransaction(Account);
        TransactionCell.Replenish(money);
    }

    public virtual void Withdraw(decimal money)
    {
        TransactionCell = new DebitTransaction(Account);
        TransactionCell.SetNext(new DepositTransaction(Account))?.SetNext(new CreditTransaction(Account));
        TransactionCell.Withdraw(money);
    }

    public virtual void Transfer(Account replenishAccount, decimal money)
    {
        TransactionCell = new DebitTransaction(Account);
        TransactionCell.SetNext(new DepositTransaction(Account))?.SetNext(new CreditTransaction(Account));
        TransactionCell.Transfer(replenishAccount, money);
    }

    public virtual void UndoTransaction()
    {
        Account.CareTaker.Restore();
    }
}