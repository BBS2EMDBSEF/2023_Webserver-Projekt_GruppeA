using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServerAppSchule.Models;

namespace ServerAppSchule.Components
{
    public partial class ProfileSlim
    {
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        UserSlim _user { get; set; }
        string _profilePic { get; set; }
        string _shortName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var currentauth = await _authenticationState;
            if (_user != null)
                _profilePic = "";
            else
                _profilePic = "";
            _shortName = currentauth.User.Identity.Name.Substring(0, 1);
        }
    }
}
