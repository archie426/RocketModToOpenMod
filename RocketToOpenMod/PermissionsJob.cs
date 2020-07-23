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

namespace RocketToOpenMod
{
    public class PermissionsJob : IJob
    {
        
        
        public async Task Do()
        {
            Console.WriteLine("[~] Loading Rocket permissions");
            FileStream stream = File.Open("Rocket.Permissions.xml", FileMode.Open);
            RocketPermissions rocket = (RocketPermissions) new XmlSerializer(typeof(RocketPermissions)).Deserialize(stream);
            stream.Close();
            
            if (rocket == null)
            {
                Console.WriteLine("[~] Could not load Rocket permissions!");
                return;
            }
            
            Console.WriteLine("[~] Preparing OpenMod permissions");

            PermissionRolesData openMod = new PermissionRolesData {Roles = new List<PermissionRoleData>()};
            
            foreach (RocketPermissionsGroup group in rocket.Groups)
                openMod.Roles.Add(await GetRoleFromRocketGroup(group));
            
            Console.WriteLine("[~] Saving OpenMod permissions");

            await SaveAsync(openMod);

        }

        private async Task<PermissionRoleData> GetRoleFromRocketGroup(RocketPermissionsGroup group)
        {
            PermissionRoleData data = new PermissionRoleData
            {
                Id = group.DisplayName,
                Parents = new HashSet<string>(),
                Priority = group.Priority,
                Permissions = new HashSet<string>()
            };

            foreach (Permission rocketPerm in group.Permissions)
                data.Permissions.Add(rocketPerm.Name);
            
            return data;

        }

        private async Task SaveAsync<T>(T data) where T : class
        {
            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            var serializedYaml = serializer.Serialize(data);
            var encodedData = Encoding.UTF8.GetBytes(serializedYaml);
            var filePath = @"OpenMod\permissions.yml";

            await File.WriteAllBytesAsync(filePath, encodedData);
            
        }
    }
}