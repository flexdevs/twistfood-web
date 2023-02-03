using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Common.Attributes;

namespace TwistFood.Service.Dtos.Accounts
{
    public class SendToPhoneNumberDto
    {
        [Required, PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
