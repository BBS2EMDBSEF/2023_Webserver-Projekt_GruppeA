using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektGruppeAWebApi.Models;

namespace ProjektGruppeAWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        #region private fields
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion
        #region public constructors
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion
        #region public methods
        [HttpPost("register")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Username);

            if (existingUser != null)
            {
                return BadRequest("Benutzername ist bereits vergeben.");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Role = model.RoleName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    if (await _roleManager.RoleExistsAsync(model.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }
                    else
                    {
                        return BadRequest("Rolle nicht gefunden.");
                    }
                }

                return Ok("Benutzer wurde erfolgreich registriert und der Rolle hinzugefügt.");
            }

            return BadRequest(result.Errors);
        }
        #endregion
    }
}
