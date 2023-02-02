using Reports.DALayer.Entities.Message;
using Reports.DALayer.Entities.Repositories.Interfaces;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly List<IMessage> _messages;

    public MessageRepository()
    {
        _messages = new List<IMessage>();
    }

    public IReadOnlyList<IMessage> Messages => _messages.AsReadOnly();
    public void Create(IMessage entity)
    {
        if (_messages.Contains(entity))
            throw new DaException("there's this message yet");
        _messages.Add(entity);
    }

    public void Delete(IMessage entity)
    {
        if (!_messages.Contains(entity))
            throw new DaException("there's no this message");
        _messages.Remove(entity);
    }

    public IMessage GetById(Guid id)
    {
        return _messages.Single(tmp => tmp.MessageId == id);
    }

    public IMessage GetLast()
    {
        return _messages.LastOrDefault(tmp => true) ?? throw new DaException("can't find last message");
    }

    public List<IMessage> GetAll()
    {
        return _messages;
    }
}