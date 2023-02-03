using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Employees;
using TwistFood.Domain.Entities.Employees;

namespace TwistFood.DataAccess.Repositories.Employees
{
    public class DeliverRepository : GenericRepository<Deliver>, IDeliverRepository
    {
        public DeliverRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
