using Banks.Entities.Client;
using Banks.Entities.MementoPattern;
using Banks.Tools;

namespace Banks.Entities.Accounts;

public abstract class Account
{
    public const int TotalDaysInMonth = 30;
    public const int DaysInYear = 365;
    public const int ValueToPercent = 100;
    protected Account(Client.Client accountOwner, decimal balance, decimal interestOnBalance)
    {
        if (balance < 0 || interestOnBalance < 0)
            throw new BanksException("Balance or interest can't be negative");
        AccountType = null;
        Balance = balance;
        Id = Guid.NewGuid();
        Owner = accountOwner ?? throw new BanksException("Client cannot be null");
        LastInterestUpdate = DateTime.Now;
        InterestOnBalance = interestOnBalance;
        CareTaker = new CareTaker(this);
    }

    public Client.Client Owner { get; }
    public virtual decimal Balance { get; internal set; }
    public Guid Id { get; }
    public virtual decimal InterestOnBalance { get; protected set; }
    public virtual decimal CurrentInterest { get; protected set; }
    public DateTime LastInterestUpdate { get; protected set; }
    public string? AccountType { get; internal set; }
    public CareTaker CareTaker { get; }

    public void ChangeInterestOnBalance(int days, DateTime updateTime)
    {
        CurrentInterest += Balance * days * InterestOnBalance / DaysInYear / ValueToPercent;
        LastInterestUpdate = updateTime;
    }

    public IMemento Save()
    {
        return new Memento(Balance);
    }

    public void Restore(IMemento memento)
    {
        if (memento is not Memento)
            throw new BanksException("Unknown Memento");

        Balance = memento.GetStatus();
    }

    public void ThrowMoneyOnBalance(decimal sum)
    {
        Balance += sum;
    }

    public void SetCurrentInterest(decimal interest)
    {
        CurrentInterest = interest;
    }

    public override string ToString()
    {
        return $"Owner: {Owner.Name} {Owner.Surname}\n" +
               $"Id: {Id}\n" +
               $"Balance: {Balance}\n" +
               $"Account type: {GetType().Name}\n";
    }
}