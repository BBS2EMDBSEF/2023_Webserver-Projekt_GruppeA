using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Transactions;

namespace ProjektGruppeApp.Services
{
    public interface IRoleService
    {
        List<string> GetNonAdminRoleNames();
        bool RoleExists(string roleName);
    }
    public class RoleService : IRoleService
    {
        #region private fields
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        #endregion
        #region public Constructors
        public RoleService(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        #endregion
        #region public Methods
        /// <summary>
        /// Ruft Alle Rollen ab, die nicht admin sind
        /// </summary>
        /// <returns>Liste aller Rollen</returns>
        public List<string> GetNonAdminRoleNames()
        {
            List<string> roles = new List<string>();
                roles = _context.Roles
                    .Select(r => r.Name)
                    .Where(r => r != "Admin")
                    .ToList();
            return roles;
        }
        public bool RoleExists(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName).Result;
        }
        #endregion

    }
}
