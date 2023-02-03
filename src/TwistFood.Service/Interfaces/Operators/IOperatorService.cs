using TwistFood.Service.Dtos.Operators;

namespace TwistFood.Service.Interfaces.Operators
{
    public interface IOperatorService
    {
        public Task<string> OperatorLoginAsync(OperatorLoginDto operatorLoginDto);
        public Task<bool> OperatorRegisterAsync(OperatorRegisterDto operatorRegisterDto);
    }
}
