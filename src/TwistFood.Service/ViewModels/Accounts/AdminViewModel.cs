using OnlineMarket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistFood.Service.ViewModels.Accounts
{
    public class AdminViewModel : BaseEntity
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string? ImagePath { get; set; }
        public string BirthDate { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string PassportSeriaNumber { get; set; } = String.Empty;
        public string Email { get; set; }   = string.Empty;
    }
}
