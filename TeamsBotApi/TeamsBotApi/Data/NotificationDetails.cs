using System;
using System.Text.Json;

namespace TeamsBotApi.Data
{
    public class NotificationDetails
    {
        public Guid BuildId { get; set; }
        public string ChannelId { get; set; }
        public WatchLevel WatchLevel { get; set; }

        public NotificationDetails()
        {
        }

        public NotificationDetails(Guid buildId, string channelId, WatchLevel watchLevel)
        {
            BuildId = buildId;
            ChannelId = channelId;
            WatchLevel = watchLevel;
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}