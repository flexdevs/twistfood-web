using TwistFood.Service.Dtos.Accounts;

namespace TwistFood.Service.Interfaces.Accounts
{
    public interface IVerifyPhoneNumberService
    {
        public Task<bool> SendCodeAsync(SendToPhoneNumberDto sendToPhoneNumberDto);

        public Task<string> VerifyPhoneNumber(VerifyPhoneNumberDto verifyPhoneNumberDto);
    }
}
