using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjektGruppeA_App.Models
{
    public class User : IdentityUser
    {
        #region public fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        #endregion
    }
}
