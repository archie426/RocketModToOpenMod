using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NinkyNonk.Shared.Environment;
using RocketToOpenMod.Data;

namespace RocketToOpenMod.API
{
    public class ExternalJobManager : IExternalJobManager
    {

        private readonly WriteFileType _write;
        
        public ExternalJobManager(WriteFileType write)
        {
            _write = write;
        }
        
        public async Task RunExternalJobs()
        {
            //default amount of jobs
            int i = 4;
            
            foreach (string file in Directory.GetFiles(Assembly.GetExecutingAssembly().Location.Replace("RocketToOpenMod.exe", "")))
            {
                if (!file.Contains("dll"))
                    return;
                
                Project.LoggingProxy.LogInfo("Loading assembly " + file + "...");
                
                Assembly assembly = Assembly.Load(file);
                
                foreach (Type t in assembly.GetTypes().Where(t => t.IsDefined(typeof(ExternalJobAttribute), false)))
                {
                    Job job = (Job) Activator.CreateInstance(t, _write);
                    if (job == null)
                        continue;
                    ExternalJobAttribute info = (ExternalJobAttribute) t.GetCustomAttribute(typeof(ExternalJobAttribute));
                    Project.LoggingProxy.LogUpdate($"{i}. {assembly.FullName}: {info?.Name}");
                    await job.RunAsync();
                    i++;
                }

            } 
        }
    }
}