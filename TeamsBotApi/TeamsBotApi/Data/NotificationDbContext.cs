using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<NotificationDetails> NotificationDetails { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }
    }
}