
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektGruppeApp.Models;
//Migration Command : dotnet ef migrations add init
//update Command : dotnet ef database update
namespace ProjektGruppeApp
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}