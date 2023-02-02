using Reports.DALayer.Entities.Message;
using Reports.DALayer.Models;
using Reports.DALayer.Models.Accounts;

namespace Reports.BiLayer.Reports_system;

public interface IReportService
{
    public Report CreateReport(EmployeeAccount account, IMessage message);
    public List<Report> GetAllReports();
    public Report GetLastRepost();
    public List<IMessage> GetRawMessages(EmployeeAccount account);
}