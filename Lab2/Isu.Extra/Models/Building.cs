using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class Building
{
    public Building(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new IsuException("There can't be lesson in this building");
        Address = address;
    }

    public string Address { get; }
}