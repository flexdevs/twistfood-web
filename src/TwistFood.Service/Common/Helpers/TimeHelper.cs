using TwistFood.Domain.Constants;

namespace TwistFood.Service.Common.Helpers;
public class TimeHelper
{
    public static DateTime GetCurrentServerTime()
    {
        var date = DateTime.UtcNow;
        return date.AddHours(TimeConstants.UTC);
    }
}
