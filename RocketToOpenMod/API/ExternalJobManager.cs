using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        
        public async Task LoadExternalJobs()
        {
            int i = 4;
            
            foreach (string file in Directory.GetFiles(Assembly.GetExecutingAssembly().Location.Replace("RocketToOpenMod.exe", "")))
            {
                if (!file.Contains("dll"))
                    return;
                
                Console.WriteLine("Loading assembly " + file);
                
                Assembly assembly = Assembly.Load(file);
                
                foreach (Type t in assembly.GetTypes().Where(t => t.IsDefined(typeof(ExternalJobAttribute), false)))
                {
                    Job job = (Job) Activator.CreateInstance(t, _write);
                    if (job == null)
                        return;
                    Console.WriteLine($"{i}. {assembly.FullName}: {job.Name}");
                    await job.DoAsync();
                    i++;
                }

            } 
        }
    }
}