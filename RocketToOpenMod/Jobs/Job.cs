using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RocketToOpenMod.Model.OpenMod.Permissions;
using RocketToOpenMod.Model.Rocket.Permissions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RocketToOpenMod.Jobs
{
    public abstract class Job
    {
        public abstract Task DoAsync();
        
        protected async Task<PermissionRoleData> GetRoleFromRocketGroup(RocketPermissionsGroup group)
        {
            PermissionRoleData data = new PermissionRoleData
            {
                Id = group.Id,
                Parents = new HashSet<string>(){group.ParentGroup},
                Priority = group.Priority,
                Permissions = new HashSet<string>(),
                Data = new Dictionary<string, object>(),
                DisplayName = group.DisplayName,
                IsAutoAssigned = group.Id == "default"
            };

            foreach (Permission rocketPerm in group.Permissions)
                data.Permissions.Add(rocketPerm.Name);
            
            return data;

        }

        protected Job(string name)
        {
            Name = name;
        }
        
        public string Name { get; }

        protected async Task<RocketPermissions> LoadRocketPermissionsAsync()
        {
            Console.WriteLine("[~] Loading Rocket permissions");
            FileStream stream = File.Open("Rocket.Permissions.xml", FileMode.Open);
            RocketPermissions rocket = (RocketPermissions) new XmlSerializer(typeof(RocketPermissions)).Deserialize(stream);
            stream.Close();
            return rocket;
        }

        protected async Task SaveAsync<T>(T data) where T : class
        {
            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            var serializedYaml = serializer.Serialize(data);
            var encodedData = Encoding.UTF8.GetBytes(serializedYaml);
            var filePath = @$"OpenMod\{Name}.yml";

            await File.WriteAllBytesAsync(filePath, encodedData);
            
        }
        
    }
}