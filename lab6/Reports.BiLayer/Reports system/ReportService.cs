using Reports.BiLayer.Authentication_system;
using Reports.BiLayer.Tools;
using Reports.DALayer.Entities.Message;
using Reports.DALayer.Entities.Repositories;
using Reports.DALayer.Models;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Models.Logger;

namespace Reports.BiLayer.Reports_system;

public class ReportService : IReportService
{
    public ReportService(ILogger logger, AuthenticationManager manager)
    {
        EmployeeRep = new EmployeeRepository();
        MessageRep = new MessageRepository();
        ReportsRep = new ReportsRepository();
        Logger = logger;
        AuthenticationManager = manager;
    }

    public ILogger Logger { get; }
    public EmployeeRepository EmployeeRep { get; }
    public MessageRepository MessageRep { get; }
    public ReportsRepository ReportsRep { get; }
    public AuthenticationManager AuthenticationManager { get; }
    public Report CreateReport(EmployeeAccount account, IMessage message)
    {
        if (!AuthenticationManager.LogIn(account.GetToken()))
            throw new BiException("Can't create report with this account");
        account.ResolveMessage(message);

        var report = new Report(message);
        account.RemoveMessage(message);

        EmployeeRep.Create(account.Employee);
        MessageRep.Create(message);
        ReportsRep.Create(report);
        return report;
    }

    public List<Report> GetAllReports()
    {
        return ReportsRep.GetAll();
    }

    public Report GetLastRepost()
    {
        return ReportsRep.GetLast();
    }

    public List<IMessage> GetRawMessages(EmployeeAccount account)
    {
        if (!AuthenticationManager.LogIn(account.GetToken()) || EmployeeRep.GetAccount(account.Employee.Id) == null)
            throw new BiException("can't finds any reports");
        return account.Messages.Where(tmp => tmp.Type == IMessage.MessageType.Resolved).ToList();
    }
}