﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ServerAppSchule.Models;
using ServerAppSchule.Services;
using System.Drawing;
using System.Net;

namespace ServerAppSchule.Components
{
    partial class SettingsUser : ComponentBase
    {
        [Parameter]
        public string uid { get; set; }
        [Parameter]
        public UserSettings? usrSettings { get; set; } = new UserSettings();
        RegisterUser? _usr { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private ISettingsService _settingsService { get; set; }
        [Inject]
        private IDialogService _dialogService { get; set; }
        private string _profilepic { get; set;}
        protected override async Task OnInitializedAsync()
        {
            _usr = _userService.GetUserById(uid);
            if(usrSettings == null)
            {
                usrSettings = _settingsService.GetSettings(uid) ?? new();
            }
            if(usrSettings.ProfilePicture != null)
            {
                _profilepic = _settingsService.GetProfilePicture(uid);
            }
        }
        async Task UploadProfilePicture(IBrowserFile file)
        {
             await _settingsService.UpdateProfilePictureAsync(file, _usr.Id);
        }
        void OpenDialog()
        {
            DialogParameters<ChangePasswordDialog> parameters = new DialogParameters<ChangePasswordDialog>();
            parameters.Add(x => x.uid, uid);
            _dialogService.Show<ChangePasswordDialog>("Passwort Ändern",parameters);
           
        }

    }
}