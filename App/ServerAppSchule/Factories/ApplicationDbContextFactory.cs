using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Models;
using ServerAppSchule.Data;

namespace ServerAppSchule.Factories
{
    public class ApplicationDbContextFactory : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(_options);
        }
        public void Dispose()
        {
            using (var context = CreateDbContext())
            {
                context.Dispose();
            }
        }

    }
}
