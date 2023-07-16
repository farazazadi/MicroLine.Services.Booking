using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidPassportIssueDateException : DomainException
{
    public override string Code => nameof(InvalidPassportIssueDateException);

    public InvalidPassportIssueDateException(Date issueDate) : base(
        $"Passport's IssueDate ({issueDate}) is not valid!")
    {
    }
    public InvalidPassportIssueDateException(string message) : base(message)
    {
    }

}