namespace CTSChatBotAPI.Dtos
{
    public class MessageDetailsDto
    {
        public required string Direction { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsFailed { get; set; }
    }
}
