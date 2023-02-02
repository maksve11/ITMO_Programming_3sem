using Shops.Tools;
namespace Shops.Models;

public class ShopName
{
    public ShopName(string shopName, string address)
    {
        if (Name == string.Empty || Address == string.Empty)
            throw new ShopException("Shop name value is empty or Address is empty");
        Address = address;
        Name = shopName;
    }

    public string Name { get; }

    public string Address { get; }
}