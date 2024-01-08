using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IUserService
    {
        Task<Task> CreateNewUserAsync(RegisterUser user);
        Task<IEnumerable<UserSlim>> GetAllMappedUsersAsync();
        RegisterUser GetUserById(string id);
        Task UpdateLastLoginRefresh(string uid);
        Task<Task> ChangePassword(string uid, string pwd);
    }
}
