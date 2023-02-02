using Shops.Models;
using Shops.Tools;

namespace Shops.Entities;

public class Shop
{
    private List<Product> _items;

    public Shop(ShopName shopName, Guid id)
    {
        Id = id;
        if (shopName.Name == string.Empty || shopName.Address == string.Empty)
            throw new ShopException("ShopName is null");
        NameOfShop = shopName;
        _items = new List<Product>();
    }

    public ShopName NameOfShop { get; }

    public IReadOnlyList<Product> Products => _items.AsReadOnly();

    public Guid Id { get; }

    public decimal GetProductPrice(string name)
    {
        if (name == string.Empty)
            throw new ShopException("Empty name product to get this price");
        return _items.First(tmp => tmp.Name == name).PriceValue.Price;
    }

    public int GetProductQuantity(string name)
    {
        if (name == string.Empty)
            throw new ShopException("Empty name product to get this quantity");
        return _items.First(tmp => tmp.Name == name).Quantity;
    }

    public void ChangeProductPrice(string name, Value newPrice)
    {
        if (newPrice.Price <= 0)
            throw new ShopException("strange new Price");
        if (!IsProductInShop(name))
        {
            throw new ShopException("There isn't this product in the shop");
        }

        _items.FirstOrDefault(tmp => tmp.Name == name)?.SetNewPrice(newPrice);
    }

    public bool EnoughProductInShop(string name, int quantity)
    {
        if (quantity <= 0)
            throw new ShopException("strange Quantity");
        if (!IsProductInShop(name))
            throw new ShopException("There isn't this product in the shop");
        return _items.Any(tmp => tmp.Name == name && tmp.Quantity >= quantity);
    }

    public decimal GetBill(List<Product> items)
    {
        decimal result = 0;
        if (items.Count == 0)
            throw new ShopException("Strange list products and quantity");
        foreach (Product item in items)
        {
            if (!EnoughProductInShop(item.Name, item.Quantity))
            {
                throw new ShopException("No product quantity for customer");
            }

            int index = _items.FindIndex(tmp => tmp.Name == item.Name);
            _items[index].SetNewQuantityAfterBuy(item.Quantity);
            result += item.PriceValue.Price * item.Quantity;
        }

        return result;
    }

    public void SellProducts(Customer customer, List<Product> items)
    {
        if (items.Count == 0)
            throw new ShopException("No products to buy");
        decimal bill = GetBill(items);
        customer.PayTheBill(bill);
    }

    public void SupplyAllProducts(Supply provider)
    {
        if (provider.ProductsToSupply.Count == 0)
            throw new ShopException("No products to Supply");
        _items = provider.SupplyAllProducts();
    }

    public void SupplyProducts(Supply provider, List<Product> items)
    {
        if (provider.ProductsToSupply.Count == 0)
            throw new ShopException("No products to Supply");
        _items = provider.SupplyProducts(items);
    }

    public bool IsProductInShop(string name)
    {
        return _items.Any(tmp => tmp.Name == name);
    }

    public Product? FindProduct(string name)
    {
        return _items.FirstOrDefault(tmp => tmp.Name == name) ?? null;
    }
}