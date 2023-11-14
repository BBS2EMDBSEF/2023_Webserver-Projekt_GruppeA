using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ProjektGruppeApp.Models;

namespace ProjektGruppeApp.Services
{
    public class ProjektAuthenticationstateProvider : AuthenticationStateProvider
    {
        #region private fields
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        #endregion
        #region public Constructors
        public ProjektAuthenticationstateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }
        #endregion
        #region public overrides
        /// <summary>
        /// Ruft den aktuellen Authentifizierungsstatus ab
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success
                    ? userSessionStorageResult.Value
                    : null;

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userSession.UserName),
                            new Claim(ClaimTypes.Role, userSession.Role),
                        }, "CustomAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }
        /// <summary>
        /// datet den Authentifizierungsstatus das Users ab
        /// </summary>
        /// <param name="userSession">Angaben der Aktuellen Session mit Username und Rolle oder bei logout null</param>
        /// <returns></returns>
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userSession.UserName),
                            new Claim(ClaimTypes.Role, userSession.Role),
                        }, "CustomAuth"));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

    }
    #endregion
}
