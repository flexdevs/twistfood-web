using TwistFood.Service.Dtos.Accounts;

namespace TwistFood.Service.Interfaces.Accounts
{
    public interface IEmailService
    {
        public Task SendAsync(EmailMessageDto emailMessageDto);

        Task VerifyPasswordAsync(ResetPasswordDto password);
    }
}
