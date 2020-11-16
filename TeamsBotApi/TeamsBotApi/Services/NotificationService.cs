using System;
using System.Threading.Tasks;
using TeamsBotApi.Data;

namespace TeamsBotApi.Services
{
    public class NotificationService
    {
        private readonly NotificationDbContext notificationDb;

        public NotificationService(NotificationDbContext notificationDb)
        {
            this.notificationDb = notificationDb;
        }

        public async Task AddNotificationAsync(Guid buildId, string channelId)
        {
            var notification = new NotificationDetails(buildId, channelId);

            notificationDb.Add(notification);
            await notificationDb.SaveChangesAsync();
        }
    }
}