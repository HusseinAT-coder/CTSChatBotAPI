using CTSChatBotAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace CTSChatBotAPI.Infrastructure
{
    public class ChatBotDBContext : DbContext
    {
        public ChatBotDBContext(DbContextOptions<ChatBotDBContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
    }
}
