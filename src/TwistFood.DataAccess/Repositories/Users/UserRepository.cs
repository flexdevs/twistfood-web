using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Users;
using TwistFood.Domain.Entities.Users;

namespace TwistFood.DataAccess.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
