using LocationLibrary.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace LocationLibrary.DI.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "DI Extension")]
    public static class DatabaseConfigExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<LocationDbContext>(opt => opt.UseInMemoryDatabase("LocationsDb"));

        }
    }
}
