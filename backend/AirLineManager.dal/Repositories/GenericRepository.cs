using AirLineManager.dal.Data;
using Microsoft.EntityFrameworkCore;

namespace AirLineManager.dal.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly AirlineDbContext _ctx;

        public GenericRepository(AirlineDbContext ctx)
        {
            _ctx = ctx;
        }

        public T Create(T entity)
        {
            _ctx.Add(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public ReturnRequest Delete(int id)
        {
            var entity = Get(id);

            if (entity != null) _ctx.Remove(entity);

            if (_ctx.SaveChanges() > 0)
            {
                return new ReturnRequest()
                {
                    Return = true,
                    MessageReturn = "Entity eliminated successfully"
                };
            }
            else
            {
                return new ReturnRequest()
                {
                    Return = false,
                    MessageReturn = "Entity not found"
                };
            }
        }

        public T? Get(int id)
        {
            return _ctx.Find<T>(id);
        }

        public T? Get(string id)
        {
            return _ctx.Find<T>(id);
        }

        public T? Get(Func<T, bool> query, string include)
        {
            return _ctx.Set<T>().Include(include).SingleOrDefault(query);
        }

        public List<T> GetByFilter(Func<T, bool> predicate)
        {
            return _ctx.Set<T>().Where(predicate).ToList();
        }

        public List<T> GetAll()
        {
            return _ctx.Set<T>().ToList();
        }
    }
}