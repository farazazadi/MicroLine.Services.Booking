using MicroLine.Services.Booking.WebApi;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MicroLine.Services.Booking.Tests.Integration.Common;

public sealed class BookingWebApplicationFactory : WebApplicationFactory<IAssemblyMarker>
{
    private MongoService? _mongoService;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            _mongoService = serviceProvider.GetRequiredService<MongoService>();

        });

        base.ConfigureWebHost(builder);
    }


    public async Task ResetDatabaseAsync()
    {
        if (_mongoService is null)
            return;

        var database = _mongoService.Database;

        var asyncCursor = await database.ListCollectionNamesAsync();
        var collectionNames = await asyncCursor.ToListAsync();

        foreach (var collectionName in collectionNames)
        {
            var collection = database.GetCollection<BsonDocument>(collectionName);
            await collection.DeleteManyAsync(_ => true);

        }
    }

    public override async ValueTask DisposeAsync()
    {
        await ResetDatabaseAsync();

        await base.DisposeAsync();
    }
}