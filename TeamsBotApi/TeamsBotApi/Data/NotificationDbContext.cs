using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<NotificationDetails> NotificationDetails { get; set; }
    }
}