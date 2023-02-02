using Reports.DALayer.Entities.Repositories.Interfaces;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee.Employee> _employees;

    public EmployeeRepository()
    {
        _employees = new List<Employee.Employee>();
    }

    public IReadOnlyList<Employee.Employee> Employees => _employees.AsReadOnly();

    public void Create(Employee.Employee entity)
    {
        if (_employees.Contains(entity))
            throw new DaException("there's this employee yet");
        _employees.Add(entity);
    }

    public void Delete(Employee.Employee entity)
    {
        if (!_employees.Contains(entity))
            throw new DaException("there's no this employee");
        _employees.Remove(entity);
    }

    public Employee.Employee GetById(Guid id)
    {
        return _employees.Single(tmp => tmp.Id == id);
    }

    public Employee.Employee GetLast()
    {
        return _employees.LastOrDefault(tmp => tmp.Id != null) ?? throw new DaException("there's no employee");
    }

    public List<Employee.Employee> GetAll()
    {
        return _employees;
    }

    public Guid GetId(string name)
    {
        return _employees.Single(tmp => tmp.Name == name).Id ?? Guid.Empty;
    }

    public EmployeeAccount GetAccount(Guid? id)
    {
        return _employees.Single(tmp => tmp.Id == id).Account ?? throw new DaException("there's no employee with this account");
    }

    public Employee.Employee GetLeaderId(string name)
    {
        return _employees.Single(tmp => tmp.Leader?.Name == name) ?? throw new DaException("there's no leader");
    }
}