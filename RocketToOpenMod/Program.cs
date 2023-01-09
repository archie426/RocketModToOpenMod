using System;
using System.IO;
using System.Threading.Tasks;
using NinkyNonk.Shared.Environment;
using RocketToOpenMod.API;
using RocketToOpenMod.Data;
using RocketToOpenMod.Jobs;

namespace RocketToOpenMod
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Project.LoggingProxy.LogProgramInfo();
                Project.LoggingProxy.Log("If you haven't already please place this file inside the Rocket folder");
            
                if (!File.Exists("Permissions.Config.Xml"))
                {
                    Project.LoggingProxy.LogFatal("Wrong folder!");
                    await Task.Delay(3000);
                    return;
                }

                Job currentJob;
                WriteFileType write;
                
                if (args.Length == 0)
                    write = WriteFileType.Yaml;
                else
                    write = args[0].ToLower() switch
                    {
                        "json" => WriteFileType.Json,
                        "xml" => WriteFileType.Xml,
                        _ => WriteFileType.Yaml
                    };
                
                Project.LoggingProxy.LogUpdate("1. Permission Formatting");
                currentJob = new RolePermissionsReformatJob(write);
                await currentJob.RunAsync();
            
                Project.LoggingProxy.LogUpdate("2. Permission Names");
                currentJob = new RolePermissionsRefactorJob(write);
                await currentJob.RunAsync();
            
                Project.LoggingProxy.LogUpdate("3. User General Data");
                currentJob = new UsersJob(write);
                await currentJob.RunAsync();
            
                Project.LoggingProxy.LogUpdate("4. Core Translations");
                currentJob = new LocalisationJob(write);
                await currentJob.RunAsync();
            
                Project.LoggingProxy.LogInfo("Finding jobs from external assemblies....");
                ExternalJobManager externalJobManager = new ExternalJobManager(write);
                await externalJobManager.RunExternalJobs();

                Project.LoggingProxy.LogSuccess("Done!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Project.LoggingProxy.LogFatal(e.Message);
                Console.ReadKey();
            }
            
            
        }
        
        
    }
}