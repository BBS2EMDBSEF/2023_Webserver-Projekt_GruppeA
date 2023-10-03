namespace ProjektGruppeAWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int appRoleId { get; set; }
        public AppRole Role { get; set; }
    }
}
