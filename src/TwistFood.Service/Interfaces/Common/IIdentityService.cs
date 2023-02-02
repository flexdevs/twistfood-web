using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistFood.Service.Interfaces.Common;
public interface IIdentityService
{
    public long? Id { get; }

    public string FullName { get; }

    public string PhoneNumber { get; }
    public string Role { get; }
}
