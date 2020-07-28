using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RocketToOpenMod.Data;

namespace RocketToOpenMod.API
{
    public class ExternalJobManager
    {

        private readonly WriteFileType _write;
        
        public ExternalJobManager(WriteFileType write)
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            _write = write;
        }

        private void CurrentDomainOnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
        {
            foreach (Type t in args.LoadedAssembly.GetTypes().Where(t => t.IsDefined(typeof(ExternalJobAttribute), false)))
            {
               Job job = (Job) Activator.CreateInstance(t, _write);
               job?.DoAsync();
            }
        }

        public async Task LoadExternalJobs()
        {
            foreach (string file in Directory.GetFiles(Assembly.GetExecutingAssembly().Location.Replace("RocketToOpenMod.exe", "")))
            {
                if (!file.Contains("dll"))
                    return;
                Assembly.Load(file);
            } 
        }
    }
}