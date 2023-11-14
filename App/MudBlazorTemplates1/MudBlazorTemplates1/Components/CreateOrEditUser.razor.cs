using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProjektGruppeApp.Models;
using ProjektGruppeApp.Services;
using System.Text.RegularExpressions;

namespace ProjektGruppeApp.Components
{
    public partial class CreateOrEditUser : ComponentBase
    {
        [Parameter]
        public RegisterUser? InputUser { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public List<string> _roles { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
            if(!Regex.IsMatch(pw, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                yield return "Password must contain at least one special character";
        }

        private string PasswordMatch(string arg)
        {
            if (InputUser.Password != arg)
                return "Passwords don't match";
            return null;
        }
    }
}
