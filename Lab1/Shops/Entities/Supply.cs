using Shops.Models;
using Shops.Tools;

namespace Shops.Entities;

public class Supply
{
    private readonly List<Product> _items;

    public Supply()
    {
        _items = new List<Product>();
    }

    public IReadOnlyList<Product> ProductsToSupply => _items.AsReadOnly();

    public void AddProductToSupply(Product item)
    {
        if (item.Name == string.Empty || item.Quantity == 0)
            throw new ShopException("You can't supply this product");
        if (FindProduct(item.Name) != null)
            throw new ShopException("There's this product in supply yet");
        _items.Add(item);
    }

    public void AddProductsToSupply(List<Product> items)
    {
        foreach (Product item in items)
        {
            if (item.Name == string.Empty || item.Quantity == 0)
                throw new ShopException("You can't supply this product");
            if (FindProduct(item.Name) != null)
                throw new ShopException("There's this product in supply yet");
            _items.Add(item);
        }
    }

    public List<Product> SupplyAllProducts()
    {
        return _items.Where(tmp => tmp.Name != string.Empty && tmp.Quantity > 0).ToList() ?? new List<Product>();
    }

    public List<Product> SupplyProducts(List<Product> items)
    {
        var listProducts = new List<Product>();
        foreach (Product item in items)
        {
            if (EnoughProductToSupply(item.Name, item.Quantity))
            {
                listProducts.Add(item);
            }
            else
            {
                throw new ShopException("No this product to supply in the shop");
            }
        }

        if (listProducts.Count == 0)
            throw new ShopException("No products to supply in this shop");
        return listProducts;
    }

    public bool EnoughProductToSupply(string name, int quantity)
    {
        if (quantity <= 0)
            throw new ShopException("strange Quantity");
        if (FindProduct(name) == null)
            throw new ShopException("There isn't this product in the provider");
        return _items.Any(tmp => tmp.Name == name && tmp.Quantity >= quantity);
    }

    private Product? FindProduct(string name)
    {
        return _items.FirstOrDefault(tmp => tmp.Name == name) ?? null;
    }
}