using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Entities.Employees;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Dtos.AccountAdmin;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces;
using TwistFood.Service.Interfaces.Admins;
using TwistFood.Service.Security;
using TwistFood.Service.ViewModels.Accounts;

namespace TwistFood.Service.Services.Admins
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork _unitOfWork;
        private IAuthManager _authManager;
        private IFileService _fileService;

        public AdminService(IUnitOfWork unitOfWork,
                            IAuthManager authManager,
                            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _authManager = authManager;
            _fileService = fileService;
        }

        public async Task<string> AdminLoginAsync(AdminLoginDto adminLoginDto)
        {
            var res = await _unitOfWork.Admins.FirstOrDefaultAsync(x => x.Email == adminLoginDto.Email);
            if (res == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Admin not found, Email is incorrect!"); }

            if (PasswordHasher.Verify(adminLoginDto.Password, res.Salt, res.PasswordHash))
                return _authManager.GenerateAdminToken(res);

            throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Password is wrong");
        }

        public async Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto)
        {
            var res = await _unitOfWork.Admins.FirstOrDefaultAsync(x => x.Email == adminRegisterDto.Email);
            if (res is not null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Operator is already exist");

            Admin admin = (Admin)adminRegisterDto;

            if (adminRegisterDto.Image is not null)
            {
                admin.ImagePath = await _fileService.SaveImageAsync(adminRegisterDto.Image);
            }

            var hashResult = PasswordHasher.Hash(adminRegisterDto.Password);

            admin.Salt = hashResult.Salt;

            admin.PasswordHash = hashResult.Hash;

            _unitOfWork.Admins.Add(admin);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0;
        }


        public async Task<bool> AdminUpdateAsync(long Id, AdminUpdateDto updateDto)
        {
            var admin = await _unitOfWork.Admins.FindByIdAsync(Id);
            if (admin is null) throw new StatusCodeException(HttpStatusCode.NotFound, "Admin Not found");
            if(!string.IsNullOrEmpty(updateDto.FirstName)) 
                admin.FirstName= updateDto.FirstName;
            if (!string.IsNullOrEmpty(updateDto.LastName))
                admin.LastName= updateDto.LastName;
            if (!string.IsNullOrEmpty(updateDto.Email))
                admin.Email = updateDto.Email;
            if(updateDto.Image is not null) 
            { 
                admin.ImagePath = await _fileService.SaveImageAsync(updateDto.Image);
            }
            _unitOfWork.Admins.Update(Id, admin);
            return (await _unitOfWork.SaveChangesAsync())>0;    
        }

        public async Task<AdminViewModel> GetAsync(long Id)
        {
            var admin = await _unitOfWork.Admins.FindByIdAsync(Id);
            if (admin is null) throw new StatusCodeException(HttpStatusCode.NotFound, "Admin Not found");

            return new AdminViewModel()
            {
                Id = admin.Id,
                BirthDate = admin.BirthDate.ToShortDateString(),
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                ImagePath = (admin.ImagePath is null) ? "" : admin.ImagePath,
                PassportSeriaNumber = admin.PassportSeriaNumber,
                PhoneNumber = admin.PhoneNumber,
            };
        }
    }
}
