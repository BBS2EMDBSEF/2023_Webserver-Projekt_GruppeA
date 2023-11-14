using Microsoft.AspNetCore.Identity;
using System.Security.Permissions;

namespace ProjektGruppeApp.Models
{
    public class RegisterUser
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Id { get; set; }
    }
    
}
