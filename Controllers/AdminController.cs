using CTSChatBotAPI.Dtos;
using CTSChatBotAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CTSChatBotAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ChatBotDBContext _dbContext;

        public AdminController(ChatBotDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/admin/conversations
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
