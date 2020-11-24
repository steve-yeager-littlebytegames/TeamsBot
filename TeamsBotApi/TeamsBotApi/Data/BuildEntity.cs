using System;
using System.Collections.Generic;
using System.Linq;
using BuildSystem;

namespace TeamsBotApi.Data
{
    public class BuildEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime QueueTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BuildStatus Status { get; set; }

        public List<StageEntity> Stages { get; set; }

        public static implicit operator BuildEntity(Build build)
        {
            return new BuildEntity
            {
                Id = build.Id,
                Name = build.Name,
                Number = build.Number,
                QueueTime = build.QueueTime,
                StartTime = build.StartTime,
                EndTime = build.EndTime,
                Status = build.Status,
                Stages = build.Stages.Cast<StageEntity>().ToList(),
            };
        }
    }
}