namespace ServerAppSchule.Interfaces
{

    public interface IRoleService
    {
        List<string> GetRoleNames();
        bool RoleExists(string roleName);
    }

}
