using Shops.Models;
using Shops.Tools;

namespace Shops.Entities;

public class Product
{
    public Product(string name, int quantity, Value priceValue)
    {
        if (quantity < 0 || priceValue.Price < 0)
            throw new ShopException("bad arguments for product");
        Name = name;
        PriceValue = priceValue;
        Quantity = quantity;
    }

    public string Name { get; }

    public Value PriceValue { get; private set; }

    public int Quantity { get; private set; }

    public void SetNewPrice(Value newPrice)
    {
        if (newPrice.Price <= 0)
            throw new ShopException("Strange price for product");
        PriceValue = newPrice;
    }

    public void SetNewQuantityAfterSupply(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ShopException("Strange new quantity after supply");
        Quantity += newQuantity;
    }

    public void SetNewQuantityAfterBuy(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ShopException("Strange new quantity after buy");
        Quantity -= newQuantity;
    }
}