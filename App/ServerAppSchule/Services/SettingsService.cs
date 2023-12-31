﻿using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using ServerAppSchule.Models;
using System.Buffers.Text;
using System.Drawing;

namespace ServerAppSchule.Services
{
    public interface ISettingsService
    {
        UserSettings? GetSettings(string uid);
        Task<bool> UpdateProfilePictureAsync(IBrowserFile profilePic, string uid);
        public string GetProfilePicture(string uid);
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
        #region private Methods
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
            string profilePicAsString = await _fileService.PicToBase64Async(profilePic);
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                var userSettings = context.UserSettings.FirstOrDefault(x => x.UserId == uid);
                if (userSettings != null)
                {
                    userSettings.ProfilePicture = profilePicAsString;
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    context.Add(new UserSettings
                    {
                        UserId = uid,
                        ProfilePicture = profilePicAsString
                    });
                    await context.SaveChangesAsync();
                    return true;
                }
            }
        }
        public string GetProfilePicture(string uid)
        {
            string profilePicAsString = string.Empty;
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                if(context.UserSettings.Any(x => x.UserId == uid))
                {
                    profilePicAsString = context.UserSettings
                        .Where(x => x.UserId == uid)
                        .FirstOrDefault().ProfilePicture ?? "";
                    if (profilePicAsString != "")
                    {
                        return String.Concat("data:image/png;base64,", profilePicAsString);
                    }
                }

            }
            return "";
        }
        #endregion
    }
}
