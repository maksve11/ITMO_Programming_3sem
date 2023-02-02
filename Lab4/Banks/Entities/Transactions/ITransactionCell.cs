using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions;

public interface ITransactionCell
{
    ITransactionCell? SetNext(ITransactionCell? cell);
    public void Replenish(decimal money);
    public void Withdraw(decimal money);
    public void Transfer(Account replenishAccount, decimal money);
    public void UndoTransaction();
}