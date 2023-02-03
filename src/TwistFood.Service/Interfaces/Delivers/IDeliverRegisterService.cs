using TwistFood.Service.Dtos;

namespace TwistFood.Service.Interfaces.Delivers
{
    public interface IDeliverRegisterService
    {
        public Task<bool> DeliverRegisterAsync(DeliverRegistrDto deliverRegistrDto);
    }
}
