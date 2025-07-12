using CTSChatBotAPI.Domain;
using CTSChatBotAPI.Dtos;
using CTSChatBotAPI.Infrastructure;
using CTSChatBotAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CTSChatBotAPI.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly ChatBotDBContext _dbContext;
        private readonly MockAIService _mockAIService;

        public MessageController(
            ChatBotDBContext dbContext,
            MockAIService mockAIService
            )
        {
            _dbContext = dbContext;
            _mockAIService = mockAIService;
        }

        // POST: /api/messages/receive
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveMessage([FromBody] MessagePayloadDto payload)
        {
            if (string.IsNullOrWhiteSpace(payload.UserId))
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(payload.Content))
                return BadRequest("Invalid content");

            // Create a new message
            // directed by the user
            var message = new Message
            {
                UserId = payload.UserId,
                Content = payload.Content,
                Direction = "in",
                Status = "received",
                Timestamp = DateTime.UtcNow
            };

            // Add a new record 
            _dbContext.Messages.Add(message);

            // Save changes
            await _dbContext.SaveChangesAsync();

            // Return acknowledgement message
            return Ok(new
            {
                Message = "Message received successfully.",
                MessageId = message.Id
            });
        }

        // POST: /api/messages/send
        [HttpPost("send")]
        public async Task<IActionResult> SendReply([FromBody] ReplyPayloadDto payload)
        {
            var previousMessages = _dbContext.Messages
                    .Where(m => m.UserId == payload.UserId && m.Direction == "in") // we can include ai replies too
                    .OrderBy(m => m.Timestamp) // --> get latest set of messages
                    .Select(m => m.Content) // avoid getting uneccessary data
                    //.TakeLast(5) // Last 5 messages
                    .ToList();

            if (!previousMessages.Any())
                return NotFound("No incoming messages found for this user.");

            // generate a reply using ai agent
            // here we are usign a mock service
            var reply = _mockAIService.GenerateReply(previousMessages, out bool isFailed);

            // Create a new message
            // Directed by ai
            var responseMessage = new Message
            {
                UserId = payload.UserId,
                Content = reply,
                Direction = "out",
                Status = "responded", // initial status 
                Timestamp = DateTime.UtcNow,
                IsFailed = isFailed
            };

            // Add new message to our db
            _dbContext.Messages.Add(responseMessage);

            // Simulate status transition to 'delivered'
            responseMessage.Status = "delivered";

            // save changes
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "AI reply sent successfully",
                Reply = reply,
                MessageId = responseMessage.Id
            });
        }
    }
}
