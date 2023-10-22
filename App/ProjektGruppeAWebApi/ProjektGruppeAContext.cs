using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ProjektGruppeAWebApi.Models;

//Migration Command : dotnet ef migrations add init
//update Command : dotnet ef database update
namespace ProjektGruppeWebApi
{
    public class ProjektGruppeAContext : DbContext
    {
        public IConfiguration configuration { get; set; }
        public ProjektGruppeAContext(DbContextOptions<ProjektGruppeAContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        public void Initialize()
        {
            if (!Database.EnsureCreated())
            {
                AppRoles.AddRange(
                    new AppRole { RoleName = "Admin" },
                    new AppRole { RoleName = "FTPUser" },
                    new AppRole { RoleName = "User" });
                SaveChanges();
                Users.AddRange(
                    new User { Username = "sa", Password = "NbE3PHNf4xrzT4", AppRoleId = AppRoles.Where(r => r.RoleName == "Admin").First().Id }
                );


            }
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<User>().ToTable("Users");
        //    modelBuilder.Entity<AppRole>().ToTable("AppRoles");
        //}
    }
}
