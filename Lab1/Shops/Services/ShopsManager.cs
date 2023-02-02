using Shops.Entities;
using Shops.Models;
using Shops.Tools;

namespace Shops.Services;

public class ShopsManager : IShopsManager
{
    private readonly List<Shop> _shops;
    private readonly List<Product> _products;
    private readonly Supply _supply;
    private Guid _idShopGenerator;

    public ShopsManager()
    {
        _idShopGenerator = Guid.NewGuid();
        _shops = new List<Shop>();
        _products = new List<Product>();
        _supply = new Supply();
    }

    public IReadOnlyList<Shop> Shops => _shops.AsReadOnly();

    public Shop CreateShop(ShopName shopName)
    {
        if (FindShop(shopName) != null)
            throw new ShopException("There's shop with this address yet");
        _idShopGenerator = Guid.NewGuid();
        Guid id = _idShopGenerator;
        var shop = new Shop(shopName, id);
        _shops.Add(shop);

        return shop;
    }

    public Product AddProduct(string name, int quantity, Value price)
    {
        if (FindProduct(name) == null)
            throw new ShopException("There's this Product yet");
        var product = new Product(name, quantity, price);
        _products.Add(product);
        _supply.AddProductToSupply(product);
        return product;
    }

    public void SupplyProductsToShop(Shop shop, Supply provider, List<Product> items)
    {
        if (FindShop(shop.Id) == null || items.Count == 0)
            throw new ShopException("Not Found Shop or Products is empty");
        shop.SupplyProducts(provider, items);
    }

    public void SupplyAllProductsToShop(Shop shop, Supply provider)
    {
        if (FindShop(shop.Id) == null)
            throw new ShopException("Not Found Shop");
        shop.SupplyAllProducts(provider);
    }

    public void PurchaseProduct(Shop shop, Customer customer, List<Product> items)
    {
        if (FindShop(shop.Id) == null)
            throw new ShopException("There's no Shop with this ID");
        shop.SellProducts(customer, items);
    }

    public Shop? FindTheCheapestShop(List<Lot> items)
    {
        Shop? resultShop = null;
        bool flag = false;
        foreach (Lot item in items)
        {
            if (FindProduct(item.Name) != null)
               throw new ShopException("No such product");

            var suitableShops = _shops.Where(tmp => tmp.EnoughProductInShop(item.Name, item.Count)).ToList();
            if (suitableShops.Count == 0)
                throw new ShopException("No list shops with this lot");
            Shop? flagShop = suitableShops.OrderBy(tmp => tmp.GetProductPrice(item.Name)).First();
            if (flag == false)
            {
                resultShop = flagShop;
                flag = true;
            }
            else
            {
                if (flagShop.Id != resultShop?.Id)
                {
                    throw new ShopException("No shop with this Lots");
                }
            }
        }

        if (resultShop == null)
            throw new ShopException("No such Shop");

        return resultShop;
    }

    public Shop? FindShop(Guid id)
    {
        return _shops.FirstOrDefault(tmp => tmp.Id.Equals(id)) ?? null;
    }

    public Shop? FindShop(ShopName name)
    {
        return _shops.FirstOrDefault(tmp => tmp.NameOfShop == name && tmp.NameOfShop.Address != name.Address) ?? null;
    }

    public List<Shop> FindShops(ShopName name)
    {
        return _shops.Where(tmp => tmp.NameOfShop == name).ToList() ?? new List<Shop>();
    }

    public Product? FindProduct(string name)
    {
        return _products.FirstOrDefault(item => item.Name == name) ?? null;
    }
}