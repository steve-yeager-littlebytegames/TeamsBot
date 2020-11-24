using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BuildSystem
{
    public class Stage
    {
        private StageStatus status;
        private StageUpdateDelegate updateAction;

        public string Name { get; }
        public Build Build { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public StageStatus Status
        {
            get => status;
            private set
            {
                status = value;
                updateAction?.Invoke(this);
            }
        }

        public TimeSpan Duration => EndTime - StartTime;

        public Stage(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        public async Task StartAsync(StageUpdateDelegate updateAction)
        {
            this.updateAction = updateAction;

            StartTime = DateTime.Now;
            Status = StageStatus.Running;

            var random = new Random();
            var isSuccess = random.NextDouble() <= 0.9f;
            var time = (int)(random.NextDouble() * 5 * 1000) + 1000;
            Debug.WriteLine($"Stage '{Name}' will take '{time}' ms and {(isSuccess ? "Succeed" : "Fail")}");
            await Task.Delay(time);

            EndTime = DateTime.Now;
            Status = isSuccess ? StageStatus.Succeeded : StageStatus.Failed;
        }

        public void Skip(StageUpdateDelegate updateAction)
        {
            this.updateAction = updateAction;
            Status = StageStatus.Skipped;
        }
    }
}