namespace CTSChatBotAPI.Dtos
{
    public class MessagePayloadDto
    {
        public required string UserId { get; set; } // No need for this param in case we have a logged in user (extract using jwt)
        public required string Content { get; set; }
    }
}
