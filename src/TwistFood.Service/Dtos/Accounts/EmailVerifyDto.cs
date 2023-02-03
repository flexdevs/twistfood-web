using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Accounts
{
    public class EmailVerifyDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int Code { get; set; }
    }
}
