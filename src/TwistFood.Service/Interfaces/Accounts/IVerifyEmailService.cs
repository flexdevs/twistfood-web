using TwistFood.Service.Dtos.Accounts;

namespace TwistFood.Service.Interfaces.Accounts
{
    public interface IVerifyEmailService
    {
        Task<bool> SendCodeAsync(SendCodeToEmailDto sendCodeToEmailDto);

        Task<bool> VerifyEmail(EmailVerifyDto emailVerifyDto);

        Task<bool> VerifyPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
