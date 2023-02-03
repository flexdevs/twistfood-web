using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Locations;
using TwistFood.Domain.Common;

namespace TwistFood.DataAccess.Repositories.Locations;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }
}