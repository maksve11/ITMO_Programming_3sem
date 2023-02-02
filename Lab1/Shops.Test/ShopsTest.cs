using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Shops.Tools;
using Xunit;

namespace Shops.Test;

public class ShopServiceTest
{
    [Theory]
    [InlineData(100, 10, 1)]
    [InlineData(10000, 99, 24)]
    [InlineData(24, 1, 1)]
    public void ProductsAreProvided_ProductsAreAvailableForPurchase(decimal customerMoney, int productCount, int productToBuyCount)
    {
        var productPrice = new Value(24);
        var person = new Customer(customerMoney);
        var shopManager = new ShopsManager();
        var product = new Product("apple", productCount, productPrice);

        Shop shop = shopManager.CreateShop(new ShopName("Diksi", "Russia, Saint-Petersburg"));

        var provider = new Supply();
        provider.AddProductToSupply(product);
        shopManager.SupplyAllProductsToShop(shop, provider);
        shopManager.PurchaseProduct(shop, person, new List<Product>() { new Product("apple", productToBuyCount, productPrice) });

        Assert.True(customerMoney - (productToBuyCount * productPrice.Price) == person.Balance);
        Assert.True(productCount - productToBuyCount == shop.GetProductQuantity("apple"));
    }

    [Fact]
    public void ProductsAreProvidedAndCustomerHasNotEnoughMoney_ThrowException()
    {
        var person = new Customer(0);
        var shopManager = new ShopsManager();

        var banana = new Product("banana", 25, new Value(1));
        Shop shop = shopManager.CreateShop(new ShopName("Diksi", "Russia, Saint-Petersburg"));

        var provider = new Supply();
        provider.AddProductToSupply(banana);
        shopManager.SupplyAllProductsToShop(shop, provider);

        Assert.Throws<ShopException>(() => shopManager.PurchaseProduct(shop, person, new List<Product> { new Product("banana", 3, new Value(1)) }));
    }

    [Fact]
    public void CustomerHasNegativeMoneyAmount_ThrowException()
    {
        Assert.Throws<ShopException>(() => new Customer(-1));
    }

    [Fact]
    public void FindTheCheapestShop_ShopFound()
    {
        var shopManager = new ShopsManager();

        Shop shop1 = shopManager.CreateShop(new ShopName("Nike", "Russia, Saint-Petersburg"));
        Shop shop2 = shopManager.CreateShop(new ShopName("Adidas", "Russia, Saint-Petersburg"));
        Shop shop3 = shopManager.CreateShop(new ShopName("Reserved", "Russia, Saint-Petersburg"));

        var snickers1 = new Product("Snickers", 2, new Value(5000));
        var snickers2 = new Product("Snickers", 3, new Value(4000));
        var snickers3 = new Product("Snickers", 5, new Value(3500));

        var provider1 = new Supply();
        provider1.AddProductToSupply(snickers1);
        shopManager.SupplyAllProductsToShop(shop1, provider1);
        var provider2 = new Supply();
        provider2.AddProductToSupply(snickers2);
        shopManager.SupplyAllProductsToShop(shop2, provider2);
        var provider3 = new Supply();
        provider3.AddProductToSupply(snickers3);
        shopManager.SupplyAllProductsToShop(shop3, provider3);

        Shop? shop = shopManager.FindTheCheapestShop(new List<Lot>() { new Lot("Snickers", 1) });
        Assert.True(shop != null);
    }

    [Fact]
    public void ChangeProductPriceInTheShop()
    {
        var shopManager = new ShopsManager();

        Shop shop1 = shopManager.CreateShop(new ShopName("Nike", "Russia, Saint-Petersburg"));
        var snickers1 = new Product("Snickers", 2, new Value(4500));
        var provider = new Supply();
        provider.AddProductToSupply(snickers1);
        shopManager.SupplyAllProductsToShop(shop1, provider);
        var newPrice = new Value(5000);
        shop1.ChangeProductPrice(snickers1.Name, newPrice);
        Assert.True(shop1.GetProductPrice(snickers1.Name) == newPrice.Price);
    }
}