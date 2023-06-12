using MediatR;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using System.Reflection;
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

app.UseHttpsRedirection();


app.MapGet("/", () => "MicroLine.Services.Booking");

app.Run();