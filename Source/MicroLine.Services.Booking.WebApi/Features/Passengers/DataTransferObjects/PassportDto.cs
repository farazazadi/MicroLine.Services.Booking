using MicroLine.Services.Booking.Domain.Common.Enums;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

internal record PassportDto
(
    FullNameDto FullName,
    Gender Gender,
    CountriesAlpha3Code CountryCode,
    string PassportNumber,
    DateOnly BirthDate,
    DateOnly IssueDate,
    DateOnly ExpiryDate
);