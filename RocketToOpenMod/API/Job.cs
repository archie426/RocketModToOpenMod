using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RocketToOpenMod.Data;
using RocketToOpenMod.Model.OpenMod.Permissions;
using RocketToOpenMod.Model.Rocket.Permissions;
using RocketToOpenMod.Model.Rocket.Translations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RocketToOpenMod.API
{
    public abstract class Job
    {
        public abstract Task DoAsync();

        private readonly WriteFileType _write;
        private readonly string _name;


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

        protected Job(WriteFileType write, string name)
        {
            _name = name;
            _write = write;
        }
        
        protected async Task<RocketPermissions> LoadRocketPermissionsAsync()
        {
            Console.WriteLine("[~] Loading Rocket permissions");
            FileStream stream = File.Open("Rocket.Permissions.xml", FileMode.Open);
            RocketPermissions rocket = (RocketPermissions) new XmlSerializer(typeof(RocketPermissions)).Deserialize(stream);
            stream.Close();
            return rocket;
        }
        
        protected async Task<TranslationList> LoadTranslationsAsync()
        {
            Console.WriteLine("[~] Loading Rocket translations");
            FileStream stream = File.Open("Rocket.Translations.xml", FileMode.Open);
            TranslationList rocket = (TranslationList) new XmlSerializer(typeof(TranslationList)).Deserialize(stream);
            stream.Close();
            return rocket;
        }

        protected async Task SaveAsync<T>(T data) where T : class
        {
            switch (_write)
            {
                case WriteFileType.Yaml:
                    await SaveYaml(data);
                    break;
                case WriteFileType.Json:
                    await SaveJson(data);
                    break;
                case WriteFileType.Xml:
                    await SaveXml(data);
                    break;
                default:
                    await SaveYaml(data);
                    break;
            }
            
        }

        private async Task SaveYaml<T>(T data) where T : class
        {
            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            var serializedYaml = serializer.Serialize(data);
            var encodedData = Encoding.UTF8.GetBytes(serializedYaml);
            var filePath = @$"OpenMod\{_name}.yml";
            await File.WriteAllBytesAsync(filePath, encodedData);
        }

        private async Task SaveXml<T>(T data) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(File.Open(@$"OpenMod\{_name}.xml", FileMode.Open), data);
        }

        private async Task SaveJson<T>(T data) where T : class
        {
            string serializedJson = JsonConvert.SerializeObject(data);
            byte[] encodedData = Encoding.UTF8.GetBytes(serializedJson);
            string filePath = @$"OpenMod\{_name}.json";
            await File.WriteAllBytesAsync(filePath, encodedData);
        }
        
    }
}