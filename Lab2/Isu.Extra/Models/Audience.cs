using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class Audience
{
    public Audience(int number, Building building)
    {
        if (number < 0)
            throw new IsuException("Audience can't be with this arguments");
        Number = number;
        Building = building;
    }

    public int Number { get; }
    public Building Building { get; }
}