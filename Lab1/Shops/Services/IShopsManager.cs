using Shops.Entities;
using Shops.Models;

namespace Shops.Services;

public interface IShopsManager
{
    Shop CreateShop(ShopName shopName);

    Product AddProduct(string name, int quantity, Value price);

    public void SupplyProductsToShop(Shop shop, Supply provider, List<Product> items);

    public void SupplyAllProductsToShop(Shop shop, Supply provider);

    void PurchaseProduct(Shop shop, Customer customer, List<Product> items);

    Shop? FindTheCheapestShop(List<Lot> items);

    Shop? FindShop(Guid id);

    Shop? FindShop(ShopName name);

    List<Shop>? FindShops(ShopName name);

    Product? FindProduct(string name);
}