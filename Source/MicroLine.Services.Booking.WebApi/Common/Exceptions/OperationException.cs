using System.Diagnostics.CodeAnalysis;

namespace MicroLine.Services.Booking.WebApi.Common.Exceptions;

internal class OperationException : ApplicationExceptionBase
{
    public override string Code => nameof(OperationException);

    public required string OperationName { get; init; }


    [SetsRequiredMembers]
    public OperationException(string operationName, string message) : base(message)
        => OperationName = operationName;
}