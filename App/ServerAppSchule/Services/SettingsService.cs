using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using ServerAppSchule.Models;
namespace ServerAppSchule.Services
{
    public interface ISettingsService
    {
        UserSettings? GetSettings(string uid);
        Task<bool> UpdateProfilePictureAsync(IBrowserFile profilePic, string uid);
    }

    public class SettingsService : ISettingsService
    {
        #region private fields
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly ApplicationDbContext _context;
        private IFileService _fileService;
        #endregion
        #region constructor
        public SettingsService(ApplicationDbContextFactory contextFactory, IFileService fileService)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
            _fileService = fileService;
        }
        #endregion
        #region public Methods
        /// <summary>
        /// Fetched Die Settings eines Users
        /// </summary>
        /// <param name="uid">User Id </param>
        /// <returns>einstellungen eines Users</returns>
        public UserSettings? GetSettings(string uid)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return context.UserSettings.FirstOrDefault(x => x.UserId == uid);
            }
        }
        /// <summary>
        /// Updated das Profilbild eines Users
        /// </summary>
        /// <param name="profilePic">Profilbild as IBrowserfile</param>
        /// <param name="uid">User Id</param>
        /// <returns></returns>
        public async Task<bool> UpdateProfilePictureAsync(IBrowserFile profilePic, string uid)
        {
            string profilePicAsString = _fileService.PicToBase64(profilePic);
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                var userSettings = context.UserSettings.FirstOrDefault(x => x.UserId == uid);
                if (userSettings != null)
                {
                    userSettings.ProfilePicture = profilePicAsString;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
        
        #endregion
    }
}
