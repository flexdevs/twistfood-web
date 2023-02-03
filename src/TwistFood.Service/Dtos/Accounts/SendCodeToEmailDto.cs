using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Accounts
{
    public class SendCodeToEmailDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
    }
}
