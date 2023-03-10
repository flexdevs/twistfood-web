using TwistFood.Domain.Entities.Users;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Account;
using TwistFood.Service.Dtos.Accounts;

namespace TwistFood.Service.Interfaces.Accounts
{
    public interface IAccountService
    {
        public Task<string> AccountLoginAsync(AccountLoginDto accountLoginDto);
        public Task<bool> AccountRegisterAsync(AccountRegisterDto accountRegisterDto);
        public Task<string> AccountUpdateAsync(AccountUpdateDto accountUpdateDto);
        public Task<bool> UserUpdateAsync(AccountUpdateDto accountUpdateDto);
        public Task<PagedList<User>> GetAllAsync(PagenationParams @params);
        public Task<PagedList<User>> GetAllForSearchAsync(string search, PagenationParams @params);
        public Task<bool> DeleteAsync(long id);
        public Task<User> GetAsync(long id);


    }
}
