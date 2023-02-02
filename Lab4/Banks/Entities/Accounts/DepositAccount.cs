using Banks.Tools;

namespace Banks.Entities.Accounts;

public class DepositAccount : Account
{
    public DepositAccount(Client.Client owner, decimal balance, decimal interestOnBalance, DateTime time)
        : base(owner, balance, interestOnBalance)
    {
        AccountType = "Deposit";
        TimeInterval = time;
    }

    public override decimal InterestOnBalance =>
        Balance switch
        {
            < 50000 => 3,
            < 100000 => 3.5M,
            >= 10000 => 4,
        };

    public DateTime TimeInterval { get; private set; }
    public bool IsWithdrawPossible(decimal value) => Balance >= value && Owner.IsVerifiedClient && TimeInterval <= LastInterestUpdate;

    public void ChangeDepositInterest(decimal interest)
    {
        if (interest <= 0) throw new BanksException("Interest can't be null or negative");
        InterestOnBalance = interest;
    }

    public override string ToString()
    {
        return $"Account type: {AccountType}\n" +
               $"Balance: {Balance}\n" +
               $"Interest on Balance: {InterestOnBalance}\n";
    }
}