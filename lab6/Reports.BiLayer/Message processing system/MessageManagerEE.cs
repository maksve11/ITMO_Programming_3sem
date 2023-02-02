using Reports.BiLayer.Tools;
using Reports.DALayer.Entities.Employee;
using Reports.DALayer.Entities.Message;
using Reports.DALayer.Entities.Repositories;
using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Models.Logger;
using Reports.DALayer.Tools;

namespace Reports.BiLayer.Message_processing_system;

public class MessageManagerEE : IMessageManager<EmployeeAccount, EmployeeAccount>
{
    private readonly List<EmployeeAccount> _employees;
    private readonly List<IMessage> _messages;

    public MessageManagerEE(ILogger logger)
    {
        _employees = new List<EmployeeAccount>();
        _messages = new List<IMessage>();
        Logger = logger;
        EmployeeRep = new EmployeeRepository();
        MessageRep = new MessageRepository();
    }

    public ILogger Logger { get; set; }
    public EmployeeRepository EmployeeRep { get; }
    public MessageRepository MessageRep { get; }
    public IMessage SendMessage(string message, EmployeeAccount sender, EmployeeAccount receiver)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BiException("can't send message with this text");
        if (sender is null || receiver is null)
            throw new BiException("Can't send message from null sender or receiver");
        var newMessage =
            new EmployeeToEmployeeMessage(sender, receiver, message, IMessage.MessageType.Open);

        sender.AddMessage(newMessage);
        receiver.AddMessage(newMessage);

        _employees.Add(sender);
        _employees.Add(receiver);
        MessageRep.Create(newMessage);

        string log = $"{message} send to {this}";
        Logger?.LogInformation(this.ToString(), log);
        return newMessage;
    }

    public void SubmitForProcessing(IMessage message, Employee employee)
    {
        if (employee.Account != null)
        {
            if (employee.Account.Messages.Contains(message))
            {
                string log = $"{message} send processing by {employee}";
                Logger?.LogInformation(this.ToString(), log);
                switch (employee.Status)
                {
                    case EmployeeStatus.JuniorManager:
                        message.ChangeStatus(1);
                        employee.Account.SendMessageToLeader(message);
                        break;
                    case EmployeeStatus.MiddleManager:
                        message.ChangeStatus(2);
                        employee.Account.SendMessageToLeader(message);
                        break;
                    default:
                        throw new BiException("Can't submit from team lead");
                }
            }
            else
            {
                throw new BiException("can't send message for processing to this employee");
            }
        }
    }
}