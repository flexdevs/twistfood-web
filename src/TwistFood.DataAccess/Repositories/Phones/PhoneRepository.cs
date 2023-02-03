using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Phones;
using TwistFood.Domain.Entities.Phones;

namespace TwistFood.DataAccess.Repositories.Phones
{
    public class PhoneRepository : GenericRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
