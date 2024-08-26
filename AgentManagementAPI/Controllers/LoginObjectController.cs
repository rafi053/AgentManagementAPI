using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


using System.Text;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginObjectController : ControllerBase
    {
        private string GenerateToken(string userIP)
        {
            // token handler can create token
            var tokenHandler = new JwtSecurityTokenHandler();

            string secretKey = "1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678"; //TODO: remove this from code
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            // token descriptor describe HOW to create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // things to include in the token
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userIP),
                    }
                ),
                // expiration time of the token
                Expires = DateTime.UtcNow.AddSeconds(30),
                // the secret key of the token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            // creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // converting the token to string
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        [HttpPost]
        public IActionResult Login(LoginObject loginObject)
        {
            if (loginObject.Id == "SimulationServer" || loginObject.Id == "MVCServer")
            {

                // getting the user (requester) IP
                string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return StatusCode(200
                    , new { token = GenerateToken(userIP) }
                    );
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new { error = "invalid credentials" });


        }
    }

}
