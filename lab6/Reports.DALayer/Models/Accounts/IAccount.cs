using Reports.DALayer.Entities.Message;

namespace Reports.DALayer.Models.Accounts;

public interface IAccount
{
    public void AddMessage(IMessage message);
    public void RemoveMessage(IMessage message);

    public int GetToken();
}