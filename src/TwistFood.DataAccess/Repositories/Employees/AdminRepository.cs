using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Employees;
using TwistFood.Domain.Entities.Employees;

namespace TwistFood.DataAccess.Repositories.Employees
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
