using AuthDemo.Mongo.Config;
using AuthDemo.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthDemo.Mongo;

public static class Extensions
{
    public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new MongoSettings();
        configuration.GetSection("Mongo").Bind( config);

        services.AddSingleton<MongoSettings>(config);
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }
}