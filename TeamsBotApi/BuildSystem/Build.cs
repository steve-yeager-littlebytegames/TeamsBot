using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem
{
    public delegate Task StageUpdateDelegate(Stage stage);

    public delegate Task BuildUpdateDelegate(Build build);

    public class Build
    {
        public event StageUpdateDelegate StageUpdateEvent;
        public event BuildUpdateDelegate BuildUpdateEvent;

        public Guid Id { get; }
        public string Name { get; }
        public int Number { get; }
        public IReadOnlyCollection<Stage> Stages { get; }

        public DateTime QueueTime { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public BuildStatus Status { get; private set; }

        public TimeSpan BuildDuration => EndTime - StartTime;

        public Build(string name, int number, IReadOnlyCollection<Stage> stages)
        {
            Id = Guid.NewGuid();
            Name = name;
            Number = number;
            Stages = stages;
        }

        public override string ToString() => $"{Name}#{Number}";

        public void Queue()
        {
            Status = BuildStatus.Queued;
            QueueTime = DateTime.Now;
        }

        public async Task StartAsync()
        {
            Status = BuildStatus.Running;
            StartTime = DateTime.Now;

            var lastStageStatus = StageStatus.None;

            foreach(var stage in Stages)
            {
                if(lastStageStatus != StageStatus.None && lastStageStatus != StageStatus.Succeeded)
                {
                    break;
                }

                try
                {
                    await stage.StartAsync();
                    lastStageStatus = stage.Status;
                }
                finally
                {
                    StageUpdateEvent?.Invoke(stage);
                }
            }

            Status = lastStageStatus == StageStatus.Succeeded ? BuildStatus.Succeeded : BuildStatus.Failed;
            EndTime = DateTime.Now;

            BuildUpdateEvent?.Invoke(this);
        }
    }
}