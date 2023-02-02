using Banks.Tools;

namespace Banks.Entities.Accounts;

public class CreditAccount : Account
{
    private decimal _balance;
    public CreditAccount(Client.Client owner, decimal balance, decimal interestOnBalance, decimal limit, decimal comission)
        : base(owner, balance, interestOnBalance)
    {
        if (limit < 0 || comission < 0)
            throw new BanksException("Can't be credit account with this limit or comission");
        Limit = limit;
        Comission = comission;
        _balance = balance;
        AccountType = "Credit";
    }

    public decimal Limit { get; private set; }
    public decimal Comission { get; private set; }

    public override decimal Balance
    {
        get => _balance < 0 ? _balance - (Comission / 100 * Math.Abs(_balance)) : _balance;
        internal set => _balance = value;
    }

    public void ChangeCreditInterest(decimal interest)
    {
        if (interest <= 0) throw new BanksException("Interest can't be null or negative");
        InterestOnBalance = interest;
    }

    public void ChangeCreditLimit(decimal limit)
    {
        if (limit <= 0) throw new BanksException("Interest can't be null or negative");
        Limit = limit;
    }

    public void ChangeCreditComission(decimal comission)
    {
        if (comission <= 0) throw new BanksException("Interest can't be null or negative");
        Comission = comission;
    }

    public bool IsWithdrawPossible(decimal value) => Math.Abs(Balance - value) < Limit && Owner.IsVerifiedClient;

    public override string ToString()
    {
        return $"Account type: {AccountType}\n" +
               $"Balance: {Balance}\n" +
               $"Credit Limit: {Limit}\n" +
               $"Comission: {Comission}\n";
    }
}