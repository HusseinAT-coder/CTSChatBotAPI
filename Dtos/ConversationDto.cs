namespace CTSChatBotAPI.Dtos
{
    public class ConversationDto
    {
        public required string UserId { get; set; }
        public List<MessageDetailsDto> Messages { get; set; } = [];
    }
}
