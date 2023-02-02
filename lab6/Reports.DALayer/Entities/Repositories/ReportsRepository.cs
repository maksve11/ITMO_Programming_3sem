using Reports.DALayer.Entities.Repositories.Interfaces;
using Reports.DALayer.Models;
using Reports.DALayer.Tools;

namespace Reports.DALayer.Entities.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly List<Report> _reports;

    public ReportsRepository()
    {
        _reports = new List<Report>();
    }

    public IReadOnlyList<Report> Reports => _reports.AsReadOnly();
    public void Create(Report entity)
    {
        if (_reports.Contains(entity))
            throw new DaException("there's this report yet");
        _reports.Add(entity);
    }

    public void Delete(Report entity)
    {
        if (!_reports.Contains(entity))
            throw new DaException("there's no this report");
        _reports.Remove(entity);
    }

    public Report GetById(Guid id)
    {
        return _reports.Single(tmp => tmp.Id == id);
    }

    public Report GetLast()
    {
        return _reports.LastOrDefault(tmp => true) ?? throw new DaException("can't find last report");
    }

    public List<Report> GetAll()
    {
        return _reports;
    }
}