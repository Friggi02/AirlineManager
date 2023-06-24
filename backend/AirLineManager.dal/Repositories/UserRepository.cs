using AirLineManager.dal.Data;
using AirLineManager.dal.Entities;

namespace AirLineManager.dal.Repositories
{
    public class UserRepository : GenericRepository<User>, IRepository<User>
    {
        public UserRepository(AirlineDbContext ctx) : base(ctx)
        {
        }
        public ReturnRequest Update(User entity)
        {
            User? old = Get(entity.Id);
            if (old != null)
            {
                old.Id = entity.Id;
                old.Email = entity.Email;
                old.IsDeleted = entity.IsDeleted;
                _ctx.SaveChanges();
                return new ReturnRequest()
                {
                    Return = true,
                    MessageReturn = "Entity updated successfully"
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
    }
}
