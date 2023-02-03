using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TwistFood.Service.Dtos.Accounts
{
    public class EmailMessageDto
    {
        [Required]
        public string To { get; set; } = string.Empty;
        [Required]
        public int Body { get; set; }
        [Required]
        public string Subject { get; set; } = string.Empty;
    }
}
