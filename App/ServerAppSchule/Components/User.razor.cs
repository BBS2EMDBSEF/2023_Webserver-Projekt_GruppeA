using Microsoft.AspNetCore.Components;
using ServerAppSchule.Models;
using ServerAppSchule.Services;

namespace ServerAppSchule.Components
{
    partial class User
    {
        [Parameter]
        public string uid { get; set; }
        [Parameter]
        public UserSettings? usrSettings { get; set; }

        RegisterUser? _usr { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private ISettingsService _settingsService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _usr = _userService.GetUserById(uid);
            if(usrSettings.ProfilePicture != null)
            {
                usrSettings.ProfilePicture = "";
            }
        }
    }
}
