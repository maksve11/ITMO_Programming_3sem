using System.Collections.ObjectModel;
using Banks.Entities.Accounts;
using Banks.Entities.Transactions;
using Banks.Models.TimeProvider;
using Banks.Tools;

namespace Banks.Entities.Banks;

public class Bank : IBank
{
    private readonly List<Client.Client> _clients = new List<Client.Client>();
    private readonly List<Account> _accounts = new List<Account>();
    public Bank(string? name, decimal debitInterest, decimal depositInterest, decimal creditInterest, decimal creditComission, int creditLimit)
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(name)) throw new BanksException("Name of Bank can't be null");
        Name = name;
        BankTime = new TimeProvider(DateTime.Now, DateTime.Now);
        _clients = new List<Client.Client>();
        _accounts = new List<Account>();
        DebitInterest = debitInterest;
        DepositInterest = depositInterest;
        CreditInterest = creditInterest;
        CreditComission = creditComission;
        CreditLimit = creditLimit;
    }

    public Guid Id { get; }
    public string? Name { get; }

    public ReadOnlyCollection<Client.Client> Clients => _clients.AsReadOnly();
    public ReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
    public TimeProvider BankTime { get; }
    public decimal DebitInterest { get; }
    public decimal CreditInterest { get; }
    public decimal DepositInterest { get; }
    public decimal CreditComission { get; }
    public decimal CreditLimit { get; }

    public Client.Client AddNewClient(Client.Client client)
    {
        if (client == null) throw new BanksException("Client can't be null");
        if (_clients.Contains(client)) throw new BanksException("Bank already has this client");
        _clients.Add(client);
        return client;
    }

    public void RemoveClient(Client.Client client)
    {
        if (client == null) throw new BanksException("Client can't be null");
        if (!_clients.Contains(client)) throw new BanksException("Bank doesn't has this client");
        _clients.Remove(client);
    }

    public Account CreateDebitAccount(Client.Client client, decimal money)
    {
        if (!_clients.Contains(client)) throw new BanksException("Can't find this client");
        var account = new DebitAccount(client, money, DebitInterest);
        _accounts.Add(account);
        client.AddAccount(account);
        return account;
    }

    public Account CreateDepositAccount(Client.Client client, decimal money, DateTime timeInterval)
    {
        if (!_clients.Contains(client)) throw new BanksException("Can't find this client");
        var account = new DepositAccount(client, money, DepositInterest, timeInterval);
        _accounts.Add(account);
        client.AddAccount(account);
        return account;
    }

    public Account CreateCreditAccount(Client.Client client, decimal money)
    {
        if (!_clients.Contains(client)) throw new BanksException("Can't find this client");
        var account = new CreditAccount(client, money, CreditInterest, CreditLimit, CreditLimit);
        _accounts.Add(account);
        client.AddAccount(account);
        return account;
    }

    public void ReplenishMoney(Guid accountId, decimal money)
    {
        UpdateAccounts();
        Account? account = Accounts.FirstOrDefault(acc => acc.Id == accountId);
        if (account is null) throw new BanksException("Can't find this account");
        var transaction = new Transaction(account);
        transaction.Replenish(money);
    }

    public void WithdrawMoney(Guid accountId, decimal money)
    {
        UpdateAccounts();
        Account? account = Accounts.FirstOrDefault(acc => acc.Id == accountId);
        if (account is null) throw new BanksException("Can't find this account");
        var transaction = new Transaction(account);
        transaction.Withdraw(money);
    }

    public void TransferMoney(Guid withdrawAccountId, Guid replenishAccountId, decimal money)
    {
        UpdateAccounts();
        Account? account1 = Accounts.FirstOrDefault(acc => acc.Id == withdrawAccountId);
        Account? account2 = Accounts.FirstOrDefault(acc => acc.Id == replenishAccountId);
        if (account1 is null || account2 is null) throw new BanksException("Can't find this accounts");
        var transaction = new Transaction(account1);
        transaction.Transfer(account2, money);
    }

    public void UndoTransaction(Guid accountId)
    {
        UpdateAccounts();
        Account? account = Accounts.FirstOrDefault(acc => acc.Id == accountId);
        if (account is null) throw new BanksException("Can't find this accounts");
        var transaction = new Transaction(account);
        transaction.UndoTransaction();
    }

    public void UpdateDebitInterest(decimal interest)
    {
        foreach (Account acc in _accounts)
        {
            if (acc is DebitAccount account)
            {
                account.ChangeDebitInterest(interest);
            }
        }
    }

    public void UpdateDepositInterest(decimal interest)
    {
        foreach (Account acc in _accounts)
        {
            if (acc is DepositAccount account)
            {
                account.ChangeDepositInterest(interest);
            }
        }
    }

    public void UpdateCreditInterest(decimal interest)
    {
        foreach (Account acc in _accounts)
        {
            if (acc is CreditAccount account)
            {
                account.ChangeCreditInterest(interest);
            }
        }
    }

    public void UpdateCreditLimit(int limit)
    {
        foreach (Account acc in _accounts)
        {
            if (acc is CreditAccount account)
            {
                account.ChangeCreditLimit(limit);
            }
        }
    }

    public void UpdateCreditComission(decimal comission)
    {
        foreach (Account acc in _accounts)
        {
            if (acc is CreditAccount account)
            {
                account.ChangeCreditComission(comission);
            }
        }
    }

    public void UpdateAccounts()
    {
        UpdateAccountInterest();
        UpdateAccountBalance();
    }

    public override string ToString()
    {
        return $"Id: {Id}\n" +
               $"Name: {Name}\n" +
               $"Number of clients {_clients.Count}\n" +
               $"Registered accounts: {_accounts.Count}\n";
    }

    private void UpdateAccountInterest()
    {
        foreach (Account account in _accounts)
        {
            if ((BankTime.CurrentTime - account.LastInterestUpdate).Days < 1)
                continue;

            account.ChangeInterestOnBalance((BankTime.CurrentTime - BankTime.LastUpdate).Days, BankTime.CurrentTime);
        }
    }

    private void UpdateAccountBalance()
    {
        foreach (Account account in _accounts)
        {
            account.ThrowMoneyOnBalance(account.CurrentInterest);
            account.SetCurrentInterest(0);
        }

        BankTime.SetLastUpdate(BankTime.CurrentTime);
    }
}