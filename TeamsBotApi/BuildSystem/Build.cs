﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem
{
    public class Build
    {
        public string Name { get; }
        public int Number { get; }
        public IReadOnlyCollection<Stage> Stages { get; }

        public DateTime QueueTime { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public BuildStatus Status { get; private set; }

        public Build(string name, int number, IReadOnlyCollection<Stage> stages)
        {
            Name = name;
            Number = number;
            Stages = stages;
        }

        public void Queue()
        {
            Status = BuildStatus.Queued;
            QueueTime = DateTime.Now;
        }

        public async Task StartAsync()
        {
            Status = BuildStatus.Running;
            StartTime = DateTime.Now;

            foreach(var stage in Stages)
            {
                await stage.StartAsync();
            }

            Status = BuildStatus.Succeeded;
            EndTime = DateTime.Now;
        }
    }
}