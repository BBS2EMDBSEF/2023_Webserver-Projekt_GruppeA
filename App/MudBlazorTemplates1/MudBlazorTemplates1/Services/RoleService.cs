using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace ProjektGruppeApp.Services
{
    public class RoleService
    {
        #region private fields
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
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
            List<string> roles = _roleManager.Roles
                .Select(r => r.Name)
                .Where(r => r != "Admin")
                .ToList();
            return  roles;
        }
        #endregion

    }
}
