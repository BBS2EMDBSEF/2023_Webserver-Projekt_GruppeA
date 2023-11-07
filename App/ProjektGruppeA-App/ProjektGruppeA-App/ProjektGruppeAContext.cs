using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektGruppeA_App.Models;


//Migration Command : dotnet ef migrations add init
//update Command : dotnet ef database update
namespace ProjektGruppeA_App
{
    public class ProjektGruppeAContext : IdentityDbContext<User, IdentityRole, string>
    {
        public IConfiguration configuration { get; set; }
        public ProjektGruppeAContext(DbContextOptions<ProjektGruppeAContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}
