using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class Lector
{
    public Lector(string name)
    {
        if (name == string.Empty)
            throw new IsuException("Strange name of Lector");
        Name = name;
    }

    public string Name { get; }
}