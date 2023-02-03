using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Operators
{
    public class OperatorLoginDto
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
        [Required, StrongPassword]
        public string Password { get; set; } = string.Empty;
    }
}
