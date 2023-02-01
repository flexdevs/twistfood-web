namespace TwistFood.Domain.Enums
{
    public enum OrderStatus
    {
        New = 0,
        InQueue = 1,
        Confirmed = 2,
        OnProcess = 3,
        OnDelivery = 4,
        InPoint = 6,
        Successful = 7,
        Cancelled = -1,
    }
}
