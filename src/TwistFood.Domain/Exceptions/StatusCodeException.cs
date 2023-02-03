using System.Net;

namespace TwistFood.Domain.Exceptions;

public class StatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public StatusCodeException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}