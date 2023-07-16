namespace MicroLine.Services.Booking.WebApi.Common.DataTransferObjects;

public record EntityAuditingDetailsDto
(
    string CreatedBy,
    DateTime CreatedAtUtc,
    string LastModifiedBy,
    DateTime? LastModifiedAtUtc
);