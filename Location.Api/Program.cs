using LocationApi.Cache;
using LocationLibrary.Contracts.Models;
using LocationLibrary.DI.Extensions;
using LocationLibrary.Repos;
using LocationLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDatabase()
    .AddCache<Cache>()
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
