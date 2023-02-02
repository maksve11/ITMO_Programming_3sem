using System.Collections.Immutable;
using Reports.DALayer.Entities.Employee;
using Reports.DALayer.Entities.Message;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Models.Accounts;

public class EmployeeAccount : IAccount
{
    private List<IMessage> _messages;
    public EmployeeAccount(string login, string password, Employee employee)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            throw new DaException("Can't create this account");
        Login = login;
        Password = password;
        Employee = employee;
        _messages = new List<IMessage>();
    }

    public string Login { get; private set; }
    public string Password { get; private set; }
    public Employee Employee { get; set; }

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
        return string.GetHashCode(string.Concat(Login + Password));
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new DaException("Can't change password on this account");
        Password = newPassword;
    }

    public void SendMessageToLeader(IMessage message)
    {
        if (Employee.Status == EmployeeStatus.TeamLead)
            throw new DaException("can't send message");
        if (Employee.Leader?.Account != null)
        {
            message.Type = IMessage.MessageType.Resolved;
            Employee.Leader.Account.AddMessage(message);
            Employee?.Account?.RemoveMessage(message);
        }
    }

    public void ResolveMessage(IMessage message)
    {
        if (Employee.Status == EmployeeStatus.JuniorManager)
            throw new DaException("can't change status message");
        message.ChangeStatus(2);
    }
}