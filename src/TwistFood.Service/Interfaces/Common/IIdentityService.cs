namespace TwistFood.Service.Interfaces.Common;
public interface IIdentityService
{
    public long? Id { get; }

    public string FullName { get; }

    public string PhoneNumber { get; }
    public string Role { get; }
}
