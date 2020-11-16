using System;

namespace BuildSystem.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            //var buildSystem = new Api.BuildFacade();
            //buildMonitor.BuildCompleteEvent += OnBuildComplete;

            //var build = buildFactory.CreateBuild("Client");
            //buildMonitor.AddBuild(build);
            //Console.ReadLine();
        }

        private static void OnBuildComplete(Build build)
        {
            Console.WriteLine($"build '{build.Name}' completed '{build.Status}'");
        }
    }
}