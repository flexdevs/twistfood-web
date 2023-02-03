using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.AccountAdmin
{
    public class AdminLoginDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, StrongPassword]
        public string Password { get; set; } = string.Empty;

    }
}
