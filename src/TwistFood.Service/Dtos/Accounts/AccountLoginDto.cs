using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Common.Attributes;

namespace TwistFood.Service.Dtos
{
    public class AccountLoginDto
    {
        /*public string? TelegramId { get; set; }*/
        [Required, PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;
        /*public string? PhoneId { get; set; }*/
    }
}
