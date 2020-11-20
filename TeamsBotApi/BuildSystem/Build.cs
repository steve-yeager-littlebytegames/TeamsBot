using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem
{
    public delegate Task BuildCompleteDelegate(Build build);

    public delegate Task StageCompleteDelegate(Build build, Stage stage);

    public class Build
    {
        public event StageCompleteDelegate StageCompleteEvent;
        public event BuildCompleteDelegate BuildCompleteEvent;

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
                    StageCompleteEvent?.Invoke(this, stage);
                }
            }

            Status = lastStageStatus == StageStatus.Succeeded ? BuildStatus.Succeeded : BuildStatus.Failed;
            EndTime = DateTime.Now;

            BuildCompleteEvent?.Invoke(this);
        }
    }
}