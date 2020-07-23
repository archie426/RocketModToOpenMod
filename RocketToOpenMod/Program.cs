﻿using System;
using System.IO;
using System.Threading.Tasks;
using RocketToOpenMod.Jobs;

namespace RocketToOpenMod
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("RocketMod to OpenMod");
            Console.WriteLine("If you haven't already please place this file inside the Rocket folder");
            
            if (!File.Exists("Rocket.Permissions.Xml"))
            {
                Console.WriteLine("Wrong folder! Exiting");
                await Task.Delay(3000);
                return;
            }

            Job currentJob;

            Console.WriteLine("1. Permissions ");
            currentJob = new PermissionsJob();
            await currentJob.Do();
            
            Console.WriteLine("2. Users");
            currentJob = new UsersJob();
            await currentJob.Do();
            
            Console.WriteLine("3. Core Translations");
            Console.WriteLine("Coming soon :p");

            Console.WriteLine("Done!");
            
            await Task.Delay(5000);


        }
        
        
    }
}