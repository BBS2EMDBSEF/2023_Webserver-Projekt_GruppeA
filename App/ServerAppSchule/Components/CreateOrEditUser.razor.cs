﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using ServerAppSchule.Models;
using ServerAppSchule.Services;
using System.Text.RegularExpressions;

namespace ServerAppSchule.Components
{
    public partial class CreateOrEditUser : ComponentBase
    {
        [Parameter]
        public RegisterUser? InputUser { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public List<string> _roles { get; set; }
        [Inject]
        private IRoleService _roleService { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        protected override void OnInitialized()
        {
            if (InputUser == null)
                InputUser = new RegisterUser();
            _roles = _roleService.GetNonAdminRoleNames();
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
            if (!Regex.IsMatch(pw, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                yield return "Password must contain at least one special character";
        }

        private string PasswordMatch(string arg)
        {
            if (InputUser == null)
                return "";
            if (InputUser.Password != arg)
                return "Passwörter stimmen nicht über ein";
            return "";
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task SubmitAsync()
        {
           
            //if(InputUser.Role == "" || InputUser.Role == null || _roleService.RoleExists(InputUser.Role))
            //{
            //    InputUser.Role = "User";
            //}
            if (!InputUser.IsValid())
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Benutzer kann nicht erstellt werden! Ungültige Angaben1");
            }
            
            Task create = await _userService.CreateNewUser(InputUser);
            if (create.IsCompletedSuccessfully)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Benutzer kann nicht erstellt werden! Ungültige Angaben2");
            }

        }
    }
}