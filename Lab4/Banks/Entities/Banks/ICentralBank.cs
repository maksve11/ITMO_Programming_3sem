using Banks.Entities.Accounts;

namespace Banks.Entities.Banks;

public interface ICentralBank
{
    public Bank RegisterBank(string? name, decimal debitInterest, decimal depositInterest, decimal creditInterest, decimal creditComission, int creditLimit);

    public Client.Client AddClientToBank(Client.Client client, Guid bankId);

    public Account CreateAccount(Client.Client client, Guid bankId, string? accountType, decimal money, DateTime timeInterval);

    public void TransferMoneyBetweenBanks(Account accountFrom, Account accountTo, decimal money);

    public void TimeTravel(DateTime future);
}