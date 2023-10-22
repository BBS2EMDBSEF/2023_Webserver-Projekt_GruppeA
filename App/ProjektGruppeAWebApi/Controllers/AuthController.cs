using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjektGruppeAWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using Microsoft.AspNetCore.Hosting.Server;

namespace ProjektGruppeAWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] Dictionary<string,string> body)
        {
            var user = new User();
            user.Username = body["Username"];
            user.Password = body["Password"];
            
            if (IsValidUser(user.Username, user.Password))
            {
                var key = Encoding.ASCII.GetBytes("IhrGeheimerSchlüsselHier");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        //new Claim(ClaimTypes.Role, user.Role.RoleName),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(ClaimTypes.Sid, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            return username == "testUser" && password == "testUser";
        }

        [HttpGet("DBPing")]
        [AllowAnonymous]
        public IActionResult GetDBPing()
        {
            Console.WriteLine("Started");
            string connectionString = "Server =127.0.0.1; Database = projektGruppeA; User Id = root;Persist Security Info=False; Connect Timeout=300";//"Server=lebedev-systems.de;Database=projektgruppea;User Id=Service;Password=Emden123;";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("erfolgreich Verbunden");
                    // SQL-Befehl zum Erstellen der Tabelle
                    string createTableSQL = "CREATE TABLE IF NOT EXISTS MeineTabelle (ID INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255))";

                    using (MySqlCommand cmd = new MySqlCommand(createTableSQL, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }


                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return BadRequest(ex.Message);
                }


            }


        }
    }
}

