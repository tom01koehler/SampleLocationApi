using LocationLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LocationLibrary.DI.Extensions
{
    public static class DatabaseConfigExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<LocationDbContext>(opt => opt.UseInMemoryDatabase("LocationsDb"));

        }
    }
}
