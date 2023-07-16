using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using System.Reflection;
using MicroLine.Services.Booking.WebApi.Common.Middleware;
using MicroLine.Services.Booking.WebApi.Features.Passengers;
using MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;
using MicroLine.Services.Booking.WebApi.Infrastructure.Mapster;
using MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

var executingAssembly = Assembly.GetExecutingAssembly();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMongoDb(executingAssembly)
    .AddMediatR(config => config.RegisterServicesFromAssembly(executingAssembly))
    .AddMapster()
    .AddRabbitMq()
    .AddInbox();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseMiddleware<ExceptionHandlingMiddleware>()
    .UseHttpsRedirection();


app.MapGet("/", () => "MicroLine.Services.Booking");

app.MapPassengerEndpoints();

app.Run();