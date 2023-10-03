using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjektGruppeAWebApi.Models;
using System.Configuration;

namespace ProjektGruppeWebApi
{
    public class ProjektGruppeAContext : DbContext
    {   
        public IConfiguration configuration { get; set; }
        public ProjektGruppeAContext(DbContextOptions<ProjektGruppeAContext> options): base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var con = configuration.GetConnectionString("ProjektGruppeA");
            optionsBuilder.UseMySQL(con);
        }
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
                    new User { Username = "sa", Password = "NbE3PHNf4xrzT4", appRoleId = AppRoles.Where(r => r.RoleName == "Admin").First().Id }
                );


            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<AppRole>().ToTable("AppRoles");
        }
    }
}
