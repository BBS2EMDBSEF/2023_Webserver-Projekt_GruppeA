using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjektGruppeAWebApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int AppRoleId { get; set; }
        [ForeignKey("AppRoleId")]
        public virtual AppRole Role { get; set; }
    }
}
