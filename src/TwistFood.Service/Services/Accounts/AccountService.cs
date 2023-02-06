using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Entities.Phones;
using TwistFood.Domain.Entities.Users;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Common.Exceptions;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Account;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.Interfaces.Common;

namespace TwistFood.Service.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _unitOfWork;
        private IAuthManager _authManager;
        private IIdentityService _identityService;

        public AccountService()
        {
        }


        public AccountService(IUnitOfWork unitOfWork,
                              IAuthManager authManager,
                              IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _authManager = authManager;
            _identityService = identityService;
        }

        public async Task<string> AccountLoginAsync(AccountLoginDto accountLoginDto)
        {
            var admin = await _unitOfWork.Admins.FirstOrDefaultAsync(x => x.PhoneNumber == accountLoginDto.PhoneNumber);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(x => x.PhoneNumber == accountLoginDto.PhoneNumber);
            if (user is null && admin is null) throw new ModelErrorException(nameof(accountLoginDto.PhoneNumber), "Bunday telefon raqam bilan foydalanuvchi mavjud emas!");

            string token = "";
            if (user is not null) token = _authManager.GenerateUserToken(user);
            if (admin is not null) { token = _authManager.GenerateAdminToken(admin); }

            return token;
        }

        public async Task<bool> AccountRegisterAsync(AccountRegisterDto accountRegisterDto)
        {
            var res = await _unitOfWork.Users.FirstOrDefaultAsync(x => x.PhoneNumber == accountRegisterDto.PhoneNumber);
            if (res is not null)
                throw new ModelErrorException(nameof(accountRegisterDto.PhoneNumber), "Bunday telefon raqam bilan foydalanuvchi mavjud!");

            User user = new User()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FullName = accountRegisterDto.FullName,
                PhoneNumber = accountRegisterDto.PhoneNumber

            };
            if (accountRegisterDto.TelegramId != null)
            {
                user.TelegramId = accountRegisterDto.TelegramId;
            }
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveChangesAsync();


            if (accountRegisterDto.PhoneId != null)
            {
                var user1 = await _unitOfWork.Users.FirstOrDefaultAsync(x => x.PhoneNumber == accountRegisterDto.PhoneNumber);

                Phone phone = new Phone()
                {
                    PhoneId = accountRegisterDto.PhoneId,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user1!.Id,
                    Status = "Active"
                };
                _unitOfWork.Phones.Add(phone);
                await _unitOfWork.SaveChangesAsync();
            }

            return true;

        }

        public async Task<string> AccountUpdateAsync(AccountUpdateDto accountUpdateDto)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(_identityService.Id!.Value);
            if (user == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "User not found!"); }

            if (accountUpdateDto.FullName is not null)
            {
                user.FullName = accountUpdateDto.FullName;
            }

            _unitOfWork.Users.Update(_identityService.Id!.Value, user);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 1)
            {
                var token = _authManager.GenerateUserToken(user);
                return token;
            }
            else
            {
                return "false";
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(id);
            if (user is null) throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");
            _unitOfWork.Users.Delete(user.Id);
            var res = await _unitOfWork.SaveChangesAsync();

            return res > 0;
        }

        public async Task<PagedList<User>> GetAllAsync(PagenationParams @params)
        {
            var query = _unitOfWork.Users.GetAll()
            .OrderBy(x => x.Id);

            return await PagedList<User>.ToPagedListAsync(query,
                @params);
        }

        public async Task<PagedList<User>> GetAllForSearchAsync(string search, PagenationParams @params)
        {
            var query = _unitOfWork.Users.GetAll().
                Where(x => x.FullName.ToLower().Contains(search.ToLower()) || x.PhoneNumber.Contains(search))
          .OrderBy(x => x.Id);

            return await PagedList<User>.ToPagedListAsync(query,
                @params);
        }

        public async Task<User> GetAsync(long id)
        {
            var res = await _unitOfWork.Users.FindByIdAsync(id);
            if (res is not null)
            {
                return res;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");
        }

        public async Task<bool> UserUpdateAsync(AccountUpdateDto accountUpdateDto)
        {
            if (accountUpdateDto.Id is not null)
            {
                if (!string.IsNullOrEmpty(accountUpdateDto.FullName))
                {
                    var user = await _unitOfWork.Users.FindByIdAsync((long)accountUpdateDto.Id);
                    if (user != null) 
                    {
                        user.FullName= accountUpdateDto.FullName;
                        _unitOfWork.Users.Update(user.Id, user);
                        var result = await _unitOfWork.SaveChangesAsync();
                        return result > 0;
                    }
                    return false;

                    
                }
                return false;
            }
            return false;
        }
    }
}
