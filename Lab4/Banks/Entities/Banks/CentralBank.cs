using System.Collections.ObjectModel;
using Banks.Entities.Accounts;
using Banks.Entities.Transactions;
using Banks.Tools;

namespace Banks.Entities.Banks;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks;

    public CentralBank()
    {
        _banks = new List<Bank>();
    }

    public ReadOnlyCollection<Bank> Banks => _banks.AsReadOnly();

    public Bank RegisterBank(string? name, decimal debitInterest, decimal depositInterest, decimal creditInterest, decimal creditComission, int creditLimit)
    {
        var newBank = new Bank(name, debitInterest, depositInterest, creditInterest, creditComission, creditLimit);
        if (_banks.Contains(newBank))
            throw new BanksException("This Bank already exist");
        _banks.Add(newBank);
        return newBank;
    }

    public Client.Client AddClientToBank(Client.Client client, Guid bankId)
    {
        Bank? bank = Banks.FirstOrDefault(bank => bank.Id == bankId);
        if (bank == null) throw new BanksException("Can't find this bank");
        return bank.AddNewClient(client);
    }

    public Account CreateAccount(Client.Client client, Guid bankId, string? accountType, decimal money, DateTime timeInterval)
    {
        Bank? bank = Banks.FirstOrDefault(bank => bank.Id == bankId);
        return accountType switch
        {
            "Debit" => bank?.CreateDebitAccount(client, money),
            "Deposit" => bank?.CreateDepositAccount(client, money, timeInterval),
            "Credit" => bank?.CreateCreditAccount(client, money),
            _ => null
        }

               ?? throw new BanksException("Can't create this account with this account type");
    }

    public void TransferMoneyBetweenBanks(Account accountFrom, Account accountTo, decimal money)
    {
        if (accountFrom is null || accountTo is null) throw new BanksException("Accounts cannot be null");
        if (!Banks.Any(bank => bank.Accounts.Contains(accountFrom))) throw new BanksException("Account is not find");
        if (!Banks.Any(bank => bank.Accounts.Contains(accountTo))) throw new BanksException("Account is not find");
        var transaction = new Transaction(accountFrom);
        transaction.Transfer(accountTo, money);
    }

    public void TimeTravel(DateTime future)
    {
        foreach (Bank bank in _banks)
        {
            bank.BankTime.ChangeTime(future);
            bank.UpdateAccounts();
        }
    }
}