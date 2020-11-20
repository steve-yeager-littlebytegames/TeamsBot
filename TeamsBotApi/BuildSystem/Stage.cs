using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BuildSystem
{
    public class Stage
    {
        public string Name { get; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public StageStatus Status { get; private set; }

        public TimeSpan Duration => EndTime - StartTime;

        public Stage(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        public async Task StartAsync()
        {
            Status = StageStatus.Running;
            StartTime = DateTime.Now;

            var random = new Random();
            var isSuccess = random.NextDouble() <= 0.8f;
            var time = (int)(random.NextDouble() * 5 * 1000);
            Debug.WriteLine($"Stage '{Name}' will take '{time}' ms and {(isSuccess ? "Succeed" : "Fail")}");
            await Task.Delay(time);

            Status = isSuccess ? StageStatus.Succeeded : StageStatus.Failed;
            EndTime = DateTime.Now;
        }
    }
}