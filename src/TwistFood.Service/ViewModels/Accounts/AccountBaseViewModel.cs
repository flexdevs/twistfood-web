using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistFood.Service.ViewModels.Accounts;
public class AccountBaseViewModel
{
    public long Id { get; set; }

    public string FullName { get; set; } = String.Empty;

    public string PhoneNumber { get; set; } = String.Empty;
}
