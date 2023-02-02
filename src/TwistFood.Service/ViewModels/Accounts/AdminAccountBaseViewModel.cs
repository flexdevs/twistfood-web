using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwistFood.Service.Interfaces.Common;

namespace TwistFood.Service.ViewModels.Accounts
{
   
    public class AdminAccountBaseViewModel
    {
        public long Id { get; set; }

        public string FullName { get; set; } = String.Empty;

        public string PhoneNumber { get; set; } = String.Empty;
        public string Role { get; set; } = string.Empty;    
    }
}
