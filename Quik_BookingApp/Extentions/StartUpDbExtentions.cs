using EFCore.AutomaticMigrations;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.DAO;

namespace Quik_BookingApp.Extentions
{
    public static class StartUpDbExtentions
    {
        public static async Task CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var quikDbContext = services.GetRequiredService<QuikDbContext>();

            //quikDbContext.Database.EnsureCreated();

            //MigrateDatabaseToLatestVersion.Execute<QuikDbContext>(quikDbContext);
            await quikDbContext.Database.MigrateAsync();


        }
    }
}
