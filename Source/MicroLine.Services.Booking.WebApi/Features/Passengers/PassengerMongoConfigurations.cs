using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal sealed class PassengerMongoConfigurations : IMongoConfiguration
{
    public void Configure()
    {
        ValueConvertor<NationalId, string>.Register(
            nationalId => nationalId.ToString(),
            nationalIdString => NationalId.Create(nationalIdString));

        ValueConvertor<ContactNumber, string>.Register(
            contactNumber => contactNumber.ToString(),
            contactNumberString => ContactNumber.Create(contactNumberString));

        ValueConvertor<PassportNumber, string>.Register(
            passportNumber => passportNumber.ToString(),
            passportNumberString => PassportNumber.Create(passportNumberString));
    }

}