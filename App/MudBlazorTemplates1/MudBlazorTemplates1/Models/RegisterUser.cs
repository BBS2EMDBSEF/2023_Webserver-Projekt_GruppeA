using Microsoft.AspNetCore.Identity;
using ProjektGruppeApp.Services;
using System.Security.Permissions;
using System.Text.RegularExpressions;

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

        
        /// <summary>
        /// Überprüft ob alle Felder ausgefüllt sind und das Passwort den Anforderungen entspricht
        /// </summary>
        /// <returns>true: wenn alles ausgefüllt und Passwort entrspicht den anoforderungen false: nicht alles ausgefüllt und passwort entspricht nicht den anforderungen</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(Email)
                && !string.IsNullOrEmpty(Role)
                && !string.IsNullOrEmpty(Firstname)
                && !string.IsNullOrEmpty(Lastname)
                && !string.IsNullOrEmpty(Password)
                && Password.Length >= 8
                && Regex.IsMatch(Password, @"[A-Z]")
                && Regex.IsMatch(Password, @"[a-z]")
                && Regex.IsMatch(Password, @"[0-9]")
                && Regex.IsMatch(Password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]")
        }
    }
    
}
