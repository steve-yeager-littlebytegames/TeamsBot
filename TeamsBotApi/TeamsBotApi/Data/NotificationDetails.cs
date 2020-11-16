using System;

namespace TeamsBotApi.Data
{
    public class NotificationDetails
    {
        public Guid BuildId { get; set; }
        public string ChannelId { get; set; }

        public NotificationDetails()
        {
        }

        public NotificationDetails(Guid buildId, string channelId)
        {
            BuildId = buildId;
            ChannelId = channelId;
        }
    }
}