using Reports.DALayer.Entities.Message;
using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Models.Accounts;

public class GuestAccount : IAccount
{
    private List<IMessage> _messages;
    public GuestAccount(ISource sourceAcc)
    {
        Source = sourceAcc;
        _messages = new List<IMessage>();
    }

    public ISource Source { get; private set; }

    public IReadOnlyList<IMessage> Messages => _messages.AsReadOnly();

    public void AddMessage(IMessage message)
    {
        if (_messages.Contains(message))
            throw new DaException("there's this message yet");
        _messages.Add(message);
    }

    public void RemoveMessage(IMessage message)
    {
        if (!_messages.Contains(message))
            throw new DaException("there's no this message");
        _messages.Remove(message);
    }

    public int GetToken()
    {
        return string.GetHashCode(Source.Author());
    }
}