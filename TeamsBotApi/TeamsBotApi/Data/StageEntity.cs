using System;
using BuildSystem;

namespace TeamsBotApi.Data
{
    public class StageEntity
    {
        public Guid BuildId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public StageStatus Status { get; private set; }

        public static implicit operator StageEntity(Stage stage)
        {
            return new StageEntity
            {
                BuildId = stage.Build.Id,
                StartTime = stage.StartTime,
                EndTime = stage.EndTime,
                Status = stage.Status,
            };
        }
    }
}