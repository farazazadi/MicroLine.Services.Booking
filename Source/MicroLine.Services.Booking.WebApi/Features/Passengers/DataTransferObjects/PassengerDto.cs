using MicroLine.Services.Booking.WebApi.Common.DataTransferObjects;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

internal record PassengerDto
(
    string Id,
    string RelatedUserExternalId,
    string NationalId,
    PassportDto Passport,
    string Email,
    string ContactNumber,
    EntityAuditingDetailsDto AuditingDetails
);