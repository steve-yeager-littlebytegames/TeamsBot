﻿using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<NotificationDetails> NotificationDetails { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationDetails>().HasKey(
                nameof(Data.NotificationDetails.BuildId),
                nameof(Data.NotificationDetails.ChannelId));
        }
    }
}