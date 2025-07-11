namespace CTSChatBotAPI.Domain
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public required string Direction { get; set; } // "in" or "out"
        public required string Status { get; set; }    // "received", "responded", "delivered"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsFailed { get; set; } = false;
    }
}
