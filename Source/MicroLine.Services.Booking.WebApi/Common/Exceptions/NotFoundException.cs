namespace MicroLine.Services.Booking.WebApi.Common.Exceptions;

internal class NotFoundException : ApplicationExceptionBase
{
    public override string Code => nameof(NotFoundException);

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object id)
        : base($"{entityName} with id ({id}) was not found!")
    {
    }
}