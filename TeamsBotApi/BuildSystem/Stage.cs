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

        public Stage(string name)
        {
            Name = name;
        }

        public async Task StartAsync()
        {
            Status = StageStatus.Running;
            StartTime = DateTime.Now;

            var time = (int)(new Random().NextDouble() * 3 * 1000);
            Debug.WriteLine($"Stage '{Name}' will take '{time}' ms.");
            await Task.Delay(time);

            Status = StageStatus.Succeeded;
            EndTime = DateTime.Now;
        }
    }
}