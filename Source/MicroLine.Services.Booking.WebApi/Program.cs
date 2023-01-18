using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var executingAssembly = Assembly.GetExecutingAssembly();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMongoDb(executingAssembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () => "MicroLine.Services.Booking");

app.Run();