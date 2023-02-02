using Shops.Tools;
namespace Shops.Entities;

public class Customer
{
    public Customer(decimal finance)
    {
        if (finance < 0)
            throw new ShopException("Customer has negative money amount");
        Balance = finance;
    }

    public decimal Balance { get; private set; }

    public void PayTheBill(decimal price)
    {
        if (Balance < price)
            throw new ShopException("There's no money to foot the bill");
        WithdrawMoney(price);
    }

    public void ThrowMoney(decimal money)
    {
        if (money == 0)
            throw new ShopException("Strange throw money value");
        Balance += money;
    }

    public void WithdrawMoney(decimal money)
    {
        if (money > Balance)
            throw new ShopException("You can't withdraw a lot of money");
        Balance -= money;
    }
}