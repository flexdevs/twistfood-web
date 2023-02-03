
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Common.Exceptions;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces.Accounts;

namespace TwistFood.Service.Services.Accounts
{
    public class VerifyPhoneNumberService : IVerifyPhoneNumberService
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _context;
        private readonly IAccountService _accountService;

        public VerifyPhoneNumberService(IMemoryCache cache, IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _cache = cache;
            _context = unitOfWork;
            _accountService = accountService;
        }
        public async Task<bool> SendCodeAsync(SendToPhoneNumberDto sendToPhoneNumberDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == sendToPhoneNumberDto.PhoneNumber);
            if (user is null) { throw new ModelErrorException(nameof(sendToPhoneNumberDto.PhoneNumber), "Bunday telefon raqam bilan foydalanuvchi mavjud emas!"); }
            Random r = new Random();
            int code = r.Next(1000, 9999);

            _cache.Set(sendToPhoneNumberDto.PhoneNumber, code, TimeSpan.FromMinutes(2));

            var client = new RestClient("https://sms.sysdc.ru/index.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("mobile_phone", sendToPhoneNumberDto.PhoneNumber);
            request.AddParameter("message", $"TwistFood Tadiqlash kodi: {code}");
            request.AddHeader("Authorization", "Bearer oILdj1OUmUqDZWXanwPIR3vFYVh7kDiDb6fFzIHh2OsBMeCM8eIbsadZCtn1ZgON");
            IRestResponse response = client.Execute(request);
            if (response.Content.ToString().Substring(11, 5) == "error")
            {
                throw new ModelErrorException(nameof(sendToPhoneNumberDto.PhoneNumber), "We are unable to send messages to this company at this time");
            }
            return true;
        }

        public async Task<string> VerifyPhoneNumber(VerifyPhoneNumberDto verifyPhoneNumberDto)
        {
            if (_cache.TryGetValue(verifyPhoneNumberDto.PhoneNumber, out int exceptedCode))
            {
                if (exceptedCode != verifyPhoneNumberDto.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong!");

                else
                {
                    return await _accountService.AccountLoginAsync(new AccountLoginDto() { PhoneNumber = verifyPhoneNumberDto.PhoneNumber });
                }
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }
    }
}
