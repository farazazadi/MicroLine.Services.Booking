using MediatR;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using System.Reflection;
using MicroLine.Services.Booking.WebApi.Infrastructure.Mapster;

var builder = WebApplication.CreateBuilder(args);

var executingAssembly = Assembly.GetExecutingAssembly();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMongoDb(executingAssembly)
    .AddMediatR(executingAssembly)
    .AddMapster();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () => "MicroLine.Services.Booking");

app.Run();