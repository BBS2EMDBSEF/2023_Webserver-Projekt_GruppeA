using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface ISettingsService
    {
        UserSettings? GetSettings(string uid);
        Task<bool> UpdateProfilePictureAsync(IBrowserFile profilePic, string uid);
        public string GetProfilePicture(string uid);
    }
}
