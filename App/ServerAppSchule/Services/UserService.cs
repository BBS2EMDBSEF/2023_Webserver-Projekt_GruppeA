using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using ServerAppSchule.Models;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ServerAppSchule.Services
{

    public interface IUserService
    {
        Task<Task> CreateNewUser(RegisterUser user);
        Task<IEnumerable<UserSlim>> GetAllMappedUsersAsync();
        RegisterUser GetUserById(string id);
    }

    public class UserService : IUserService
    {
        #region private fields
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        IConfiguration _configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
        private readonly ApplicationDbContextFactory _contextFactory;
        #endregion
        #region constructor
        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ApplicationDbContextFactory contextFactory)
        {

            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
            _userManager = userManager;
        }
        #endregion
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
            var user = new User
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                EmailConfirmed = registerUser.EmailConfirmed,

            };
            await _userManager.CreateAsync(user, registerUser.Password);
            await _userManager.AddToRoleAsync(user, registerUser.Role);
        }
        /// <summary>
        /// Erstellt einen User in MySQL
        /// </summary>
        /// <param name="registerUser">User Daten die vom Eingabe Formular übergeben wurden</param>
        private void CreateMySQLUser(RegisterUser registerUser)
        {
            string database = "projektgruppea";
            string connectionString = _configuration.GetConnectionString("MySQLBase");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandText = "CREATE USER @username IDENTIFIED BY @password";
                        cmd.Parameters.AddWithValue("@username", registerUser.UserName);
                        cmd.Parameters.AddWithValue("@password", registerUser.Password);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"GRANT ALL PRIVILEGES ON `{database}`.* TO @username";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@username", registerUser.UserName);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "FLUSH PRIVILEGES";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine($"User '{registerUser.UserName}' created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
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
                //CreateSysUser(user);
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
            using var context = _contextFactory.CreateDbContext();
            var users = await context.Users.ToListAsync();
            var userRoles = await context.UserRoles.ToListAsync();
            var roles = await context.Roles.ToListAsync();
            IEnumerable<Task<UserSlim>> userSlimList = users.Select(async user =>
            {
                var userSlim = new UserSlim
                {
                    Email = user.Email,
                    Role = roles.First(r => r.Id == userRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId).Name ?? string.Empty,
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
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            var roles = context.Roles.ToList();
            //var usersWithRoles = context.Users
            //        .Include(u => u.UserRoles)
            //            .ThenInclude(ur => ur.Role)
            //        .ToList();
            RegisterUser registerUser = new RegisterUser
            {
                Email = user.Email,
                UserName = user.UserName,
                Role = roles.Find(roles => roles.Id == context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId).Name ?? string.Empty,
                Id = user.Id
            };
            return registerUser;
        }
        #endregion

    }

}
