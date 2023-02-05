using TwistFood.Service.Dtos.AccountAdmin;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.ViewModels.Accounts;

namespace TwistFood.Service.Interfaces.Admins
{
    public interface IAdminService
    {
        public Task<string> AdminLoginAsync(AdminLoginDto adminLoginDto);
        public Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto);
        public Task<AdminViewModel> GetAsync(long Id);
        public Task<bool> AdminUpdateAsync();
    }
}
