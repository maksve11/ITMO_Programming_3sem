using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Client;
using Banks.Tools;
using Xunit;

namespace Banks.Test;

public class BanksTests
{
    [Fact]
    public void AddClientWithDebitAccountTest()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        Client maksim = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account = centralBank.CreateAccount(maksim, tinkoff.Id, "Debit", 2000, DateTime.Today);
        Assert.Equal("Debit", account.AccountType);
        Assert.Equal(2000, account.Balance);
    }

    [Fact]
    public void AddClientWithCreditAccountTest()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        Client maksim = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account = centralBank.CreateAccount(maksim, tinkoff.Id, "Credit", 10000, DateTime.Today);
        Assert.Equal(maksim, account.Owner);
        Assert.Equal(10000, account.Balance);
    }

    [Fact]
    public void CreditTransactionReplenishTest()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        Client maksim = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account = centralBank.CreateAccount(maksim, tinkoff.Id, "Credit", 10000, DateTime.Today);
        tinkoff.ReplenishMoney(account.Id, 10000);
        Assert.Equal(20000, account.Balance);
    }

    [Fact]
    public void CreditTransactionWithdrawExceptionTest()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        Client maksim = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account = centralBank.CreateAccount(maksim, tinkoff.Id, "Credit", 1500, DateTime.Today);
        Assert.Throws<BanksException>(() =>
        {
            tinkoff.WithdrawMoney(account.Id, 3100);
        });
    }

    [Fact]
    public void TransferTest()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        var client1 = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account1 = centralBank.CreateAccount(client1, tinkoff.Id, "Debit", 200, DateTime.Today);
        var client2 = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Egor", "Magomedov").SetAddress("Вязьма").SetPassport(1231312212).Build(), tinkoff.Id);
        var account2 = centralBank.CreateAccount(client2, tinkoff.Id, "Credit", 1000, DateTime.Today);

        tinkoff.TransferMoney(account1.Id, account2.Id, 100);

        Assert.Equal(100, account1.Balance);
        Assert.Equal(1100, account2.Balance);
    }

    [Fact]
    public void UndoTransaction()
    {
        var centralBank = new CentralBank();
        Bank tinkoff = centralBank.RegisterBank("Tinkoff", 3, 4, 7, new decimal(1.5), 1000000);
        var client1 = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Maksim", "Velichko").SetAddress("Вязьма").SetPassport(1213123456).Build(), tinkoff.Id);
        var account1 = centralBank.CreateAccount(client1, tinkoff.Id, "Debit", 200, DateTime.Today);
        var client2 = centralBank.AddClientToBank(new ClientBuilder().SetNameAndSurname("Egor", "Magomedov").SetAddress("Вязьма").SetPassport(1231312212).Build(), tinkoff.Id);
        var account2 = centralBank.CreateAccount(client2, tinkoff.Id, "Credit", 1000, DateTime.Today);

        tinkoff.TransferMoney(account1.Id, account2.Id, 100);
        tinkoff.UndoTransaction(account1.Id);

        Assert.Equal(200, account1.Balance);
    }
}