using Banks.Entities.Accounts;
using Banks.Entities.Client;

namespace Banks.Entities.Banks;

public interface IBank
{
    public Client.Client AddNewClient(Client.Client client);

    public void RemoveClient(Client.Client client);

    public Account CreateDebitAccount(Client.Client client, decimal money);

    public Account CreateDepositAccount(Client.Client client, decimal money, DateTime timeInterval);

    public Account CreateCreditAccount(Client.Client client, decimal money);

    public void ReplenishMoney(Guid accountId, decimal money);

    public void WithdrawMoney(Guid accountId, decimal money);

    public void TransferMoney(Guid withdrawAccountId, Guid replenishAccountId, decimal money);

    public void UndoTransaction(Guid accountId);

    public void UpdateDebitInterest(decimal interest);

    public void UpdateDepositInterest(decimal interest);

    public void UpdateCreditInterest(decimal interest);

    public void UpdateCreditLimit(int limit);

    public void UpdateCreditComission(decimal comission);
}