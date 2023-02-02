using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Message;

public class GuestToEmployeeMessage : IMessage
{
    public GuestToEmployeeMessage(GuestAccount source, EmployeeAccount employee, string messageText, IMessage.MessageType type)
    {
        if (string.IsNullOrWhiteSpace(messageText))
            throw new DaException("Can't be null message text");
        if (source == null || employee == null)
            throw new DaException("Can't ne null author or receiver");
        Author = source;
        Receiver = employee;
        MessageText = messageText;
        SendTime = DateTime.Now;
        Type = type;
        MessageId = Guid.NewGuid();
    }

    public GuestAccount Author { get; set; }
    public EmployeeAccount Receiver { get; set; }
    public string MessageText { get; set; }
    public DateTime SendTime { get; set; }
    public IMessage.MessageType Type { get; set; }
    public Guid MessageId { get; set; }

    public void ChangeStatus(int status)
    {
        switch (status)
        {
            case 0:
                Type = IMessage.MessageType.Open;
                break;
            case 1:
                Type = IMessage.MessageType.Active;
                break;
            case 2:
                Type = IMessage.MessageType.Resolved;
                break;
            default:
                throw new DaException("Can't change status");
        }
    }
}