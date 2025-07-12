using CTSChatBotAPI.Dtos;
using CTSChatBotAPI.Dtos.Admin;
using CTSChatBotAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CTSChatBotAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ChatBotDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public AdminController(ChatBotDBContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult AdminLogin([FromBody] CredentialsPayloadDto payload)
        {
            // Usually we validate against database/identity
            // throw an exception in case its not a user of role admin
            if (payload.Username != "admin" || payload.Password != "password")
                return Unauthorized("Invalid credentials");

            var jwtConfig = _configuration.GetSection("Jwt"); // load jwt Enviromental variable

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!)); // encryption key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // signing credentials

            // Generate a new token here
            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, payload.Username), // adding a claims, allow us to access token paylaod easier 
                    new Claim(ClaimTypes.Role, "Admin") // add admin role, applying endpoint authorization
                },
                expires: DateTime.UtcNow.AddHours(1), // Add expiry date according to your specifications 
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // GET: /api/admin/conversations
        [Authorize(Roles = "Admin")]
        [HttpGet("conversations")]
        public IActionResult GetConversations()
        {
            // get all messages
            var grouped = _dbContext.Messages
                .OrderBy(m => m.Id) // order by id ==> Asc
                .GroupBy(m => m.UserId) // group by user
                // Map records into conversation dto
                .Select(g => new ConversationDto
                {
                    UserId = g.Key,
                    Messages = g.Select(m => new MessageDetailsDto
                    {
                        Direction = m.Direction,
                        Content = m.Content,
                        Timestamp = m.Timestamp,
                        IsFailed = m.IsFailed,
                    }).ToList()
                })
                .ToList();

            return Ok(grouped);
        }
    }
}
