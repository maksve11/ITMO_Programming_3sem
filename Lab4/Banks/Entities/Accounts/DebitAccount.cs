using Banks.Tools;

namespace Banks.Entities.Accounts;

public class DebitAccount : Account
{
    public DebitAccount(Client.Client owner, decimal balance, decimal interestOnBalance)
        : base(owner, balance, interestOnBalance)
    {
        if (interestOnBalance == 0) throw new BanksException("Interest on balance cannot be null or negative");
        AccountType = "Debit";
        InterestOnBalance = interestOnBalance;
    }

    public bool IsWithdrawPossible(decimal value) => Balance >= value && Owner.IsVerifiedClient;

    public void ChangeDebitInterest(decimal interest)
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