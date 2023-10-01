namespace ProjektGruppeAWebApi.Models
{
    public class UserFull : UserLogin
    {
        public int Id { get; set; }
        public string appRoleId { get; set; }

    }
}
