using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Sources;

public class MessengerSource : ISource
{
    public MessengerSource(string name, string messengerName)
    {
        if (string.IsNullOrWhiteSpace(messengerName) || string.IsNullOrWhiteSpace(name))
            throw new DaException("can't be this messenger or name");
        Messenger = messengerName;
        Name = name;
        Id = Guid.NewGuid();
    }

    public string Messenger { get; private set; }
    public string Name { get; set; }
    public Guid Id { get; set; }

    public string Author()
    {
        return string.Concat(Name, Messenger);
    }
}