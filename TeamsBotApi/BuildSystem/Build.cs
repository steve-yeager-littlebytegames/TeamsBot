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

        private BuildStatus status;

        public Guid Id { get; }
        public string Name { get; }
        public int Number { get; }
        public IReadOnlyCollection<Stage> Stages { get; }

        public DateTime QueueTime { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public BuildStatus Status
        {
            get => status;
            private set
            {
                status = value;
                BuildUpdateEvent?.Invoke(this);
            }
        }

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
            QueueTime = DateTime.Now;
            Status = BuildStatus.Queued;
        }

        public async Task StartAsync()
        {
            StartTime = DateTime.Now;
            Status = BuildStatus.Running;

            var lastStageStatus = StageStatus.None;

            foreach(var stage in Stages)
            {
                if(lastStageStatus == StageStatus.Failed || lastStageStatus == StageStatus.Cancelled)
                {
                    stage.Skip(OnStageUpdate);
                }
                else
                {
                    await stage.StartAsync(OnStageUpdate);
                    lastStageStatus = stage.Status;
                }
            }

            EndTime = DateTime.Now;
            Status = lastStageStatus == StageStatus.Succeeded ? BuildStatus.Succeeded : BuildStatus.Failed;
        }

        private Task OnStageUpdate(Stage stage)
        {
            return StageUpdateEvent?.Invoke(stage);
        }
    }
}