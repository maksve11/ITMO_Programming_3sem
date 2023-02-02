using Reports.DALayer.Models.Accounts;

namespace Reports.DALayer.Entities.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee.Employee>
{
    public Guid GetId(string name);
    public EmployeeAccount GetAccount(Guid? id);
    public Employee.Employee GetLeaderId(string name);
}