using OnlineMarket.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace TwistFood.Service.Dtos.Accounts
{
    public class AccountUpdateDto 
    {
        public long? Id { get; set; }
        [MaxLength(70),MinLength(2)]
        public string? FullName { get; set; }
    }
}
