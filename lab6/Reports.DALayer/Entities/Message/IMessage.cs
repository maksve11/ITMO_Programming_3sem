namespace Reports.DALayer.Entities.Message;

public interface IMessage
{
    public enum MessageType
    {
        Open = 0,
        Active = 1,
        Resolved = 2,
    }

    public string MessageText { get; set; }
    public DateTime SendTime { get; set; }
    public MessageType Type { get; set; }
    public Guid MessageId { get; set; }

    public void ChangeStatus(int status);
}