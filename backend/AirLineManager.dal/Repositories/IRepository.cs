namespace AirLineManager.dal.Repositories
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        T? Get(string id);
        T? Get(Func<T, bool> query, string include);
        List<T> GetAll();
        T Create(T entity);
        ReturnRequest Delete(int id);
        List<T> GetByFilter(Func<T, bool> predicate);
        ReturnRequest Update(T entity);
    }
}