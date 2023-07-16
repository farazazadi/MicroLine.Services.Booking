namespace MicroLine.Services.Booking.WebApi.Common.Exceptions;

internal abstract class ApplicationExceptionBase : Exception
{
    public abstract string Code { get; }
    protected ApplicationExceptionBase(string message) : base(message) { }
}
