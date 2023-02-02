using Shops.Tools;

namespace Shops.Models;

public record Value
{
    public Value(decimal price)
    {
        if (price < 0)
            throw new ShopException("Price can't be negative");
        Price = price;
    }

    public decimal Price { get; }
}