using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Sources;

public class MobileSource : ISource
{
    public MobileSource(string name, long mobilePhone)
    {
        if (string.IsNullOrWhiteSpace(name) || mobilePhone <= 0)
            throw new DaException("can't be this mail or name");
        MobilePhone = mobilePhone;
        Name = name;
        Id = Guid.NewGuid();
    }

    public string Name { get; set; }
    public long MobilePhone { get;  private set; }
    public Guid Id { get; set; }

    public string Author()
    {
        return string.Concat(Name, MobilePhone);
    }
}