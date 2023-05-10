using LocationLibrary.Contracts.Models;
using LocationLibrary.DI.Extensions;
using LocationLibrary.Repos;
using LocationLibrary.Services;
using System.Diagnostics.CodeAnalysis;

namespace LocationApi
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddDatabase()
            .AddCache<LocationApi.Cache.Cache>()
            .AddScoped<IRepo<Location>, LocationRepo>()
            .AddScoped<ILocationService, LocationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}