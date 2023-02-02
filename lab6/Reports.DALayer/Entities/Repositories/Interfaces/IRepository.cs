using Reports.DALayer.Entities.Message;
using Reports.DALayer.Models.Accounts;

namespace Reports.DALayer.Entities.Repositories.Interfaces;

public interface IRepository<T>
    where T : class
{
    public void Create(T entity);
    public void Delete(T entity);
    public T GetById(Guid id);
    public T GetLast();
    public List<T> GetAll();
}