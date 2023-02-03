using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Employees;
using TwistFood.Domain.Entities.Employees;

namespace TwistFood.DataAccess.Repositories.Employees
{
    public class OperatorRepository : GenericRepository<Operator>, IOperatorRepository
    {
        public OperatorRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
