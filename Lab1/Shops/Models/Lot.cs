using Shops.Tools;

namespace Shops.Models;

public class Lot
{
    public Lot(string name, int count)
    {
        if (name == string.Empty || count < 0)
            throw new ShopException("Wrong name or count of lot");
        Name = name;
        Count = count;
    }

    public string Name { get; private set; }
    public int Count { get; private set; }
}