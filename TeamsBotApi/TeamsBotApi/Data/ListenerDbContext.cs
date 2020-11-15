using System;
using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class BuildEventListener
    {
        public Guid BuildId { get; set; }
        public string ChannelId { get; set; }
    }

    public class ListenerDbContext : DbContext
    {

    }
}