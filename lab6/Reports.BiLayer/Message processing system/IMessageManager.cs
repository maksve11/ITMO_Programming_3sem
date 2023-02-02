using Reports.DALayer.Entities.Employee;
using Reports.DALayer.Entities.Message;
using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;

namespace Reports.BiLayer.Message_processing_system;

public interface IMessageManager<in T, in TK>
    where T : IAccount
{
    public IMessage SendMessage(string message, T sender, TK receiver);
    public void SubmitForProcessing(IMessage message, Employee employee);
}