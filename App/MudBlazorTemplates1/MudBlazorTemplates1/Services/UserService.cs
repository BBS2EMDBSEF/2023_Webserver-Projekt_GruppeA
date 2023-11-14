using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using ProjektGruppeApp;
using ProjektGruppeApp.Models;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjektGruppeApp.Services
{

    public class UserService
    {
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        IConfiguration _configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #region private Methods
        /// <summary>
        /// erstel einen User für das Linux System
        /// </summary>
        /// <param name="user">User Daten die vom Eingabe Formular übergeben wurden</param>
        private void CreateSysUser(RegisterUser user)
        {
            Process process = new Process();
            process.StartInfo.FileName = "sudo";
            process.StartInfo.Arguments = $"useradd -m {user.UserName}";
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = $"chpasswd";
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.WriteLine($"{user.UserName}:{user.Password}");
            process.StandardInput.Close();
            process.WaitForExit();
        }
        /// <summary>
        /// erstellt einen User der sich in der App anmelden kann
        /// </summary>
        /// <param name="registerUser">User Daten die vom Eingabe Formular übergeben wurden</param>
        /// <returns></returns>
        private async Task CreateAppUserAsync(RegisterUser registerUser)
        {
            if (!_context.Users.Any())
            {
                var user = new User
                {
                    Email = registerUser.Email,
                    UserName = registerUser.UserName,
                    EmailConfirmed = registerUser.EmailConfirmed,
                    Firstname = registerUser.Firstname,
                    Lastname = registerUser.Lastname,

                };
                await _userManager.CreateAsync(user, registerUser.Password);
                await _userManager.AddToRoleAsync(user, registerUser.Role);
            }
        }
        /// <summary>
        /// Erstellt einen User in MySQL
        /// </summary>
        /// <param name="registerUser">User Daten die vom Eingabe Formular übergeben wurden</param>
        private void CreateMySQLUser(RegisterUser registerUser)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string userName = registerUser.UserName;
                string password = registerUser.Password;
                string email = registerUser.Email;
                string insertQuery = "INSERT INTO Users (UserName, Password, Email) VALUES (@UserName, @Password, @Email)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        #endregion
        #region public methods
        /// <summary>
        /// erstellt einen User
        /// </summary>
        /// <param name="user">Daten aus dem Formular</param>
        /// <returns>Task Status ob erstellen geklappt hat</returns>
        public async Task<Task> CreateNewUser(RegisterUser user)
        {
            try
            {
                await CreateAppUserAsync(user);
                CreateSysUser(user);
                CreateMySQLUser(user);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
        /// <summary>
        /// Gibt Alle User im gemappten Zustand wieder
        /// </summary>
        /// <returns>Liste an Usern</returns>
        public async Task<IEnumerable<UserSlim>> GetAllMappedUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            var userSlimList = users.Select(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userSlim = new UserSlim
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    Role = roles.Any() 
                        ? roles.First() 
                        : "",
                    Username = user.UserName,
                    Id = user.Id
                };
                return userSlim;
            });

            return await Task.WhenAll(userSlimList);
        }
        /// <summary>
        /// Sucht den User anhand der ID und gibt ihn im gemappten Zustand wieder
        /// </summary>
        /// <param name="id">Id des Users der Herausgesucht werden soll</param>
        /// <returns>User</returns>
        public RegisterUser GetUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var roles = _userManager.GetRolesAsync(user).Result;
            RegisterUser registerUser = new RegisterUser
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserName = user.UserName,
                Role = roles.Any() 
                    ? roles.First() 
                    : "",
                Id = user.Id
            };
            return registerUser;
        }
        #endregion

    }

}
