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
        public string Name { get; }

        protected async Task LogInfo(object input) {
          Console.WriteLine("[~] " + input);
        }

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

        public override string ToString()
        {
            return Name;
        }
        
        protected Job(WriteFileType write, string name)
        {
            Name = name;
            _write = write;
        }
        
        protected async Task<RocketPermissions> LoadRocketPermissionsAsync()
        {
            LogInfo("Loading Rocket permissions");
            return await DeserializeRocketAsset<RocketPermissions>("Permissions.Config.xml");
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected async Task<T> DeserializeRocketAsset<T>(string name)
        {
            FileStream stream = File.Open(name, FileMode.Open);
            T rocket = (T) new XmlSerializer(typeof(T)).Deserialize(stream);
            stream.Close();
            return rocket;
        }
        
        protected async Task<TranslationList> LoadTranslationsAsync()
        {
            LogInfo("Loading Rocket translations");
            return await DeserializeRocketAsset<TranslationList>("Rocket.Translations.en.xml");
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
            ISerializer serializer = new SerializerBuilder().EmitDefaults()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            var serializedYaml = serializer.Serialize(data);
            var encodedData = Encoding.UTF8.GetBytes(serializedYaml);
            var filePath = @$"{Name}.yml";
            if (!File.Exists(filePath))
                await File.Create(filePath).DisposeAsync();
            await File.WriteAllBytesAsync(filePath, encodedData);
        }

        protected async Task<PermissionRolesData> LoadOpenPermissionsAsync()
        {
            LogInfo("Loading OpenMod permissions");
            IDeserializer serializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            return (PermissionRolesData) serializer.Deserialize(new StreamReader(File.Open("permissions.yml", FileMode.Open)));
        }

        private async Task SaveXml<T>(T data) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(File.Open(@$"{Name}.xml", FileMode.Open), data);
        }

        private async Task SaveJson<T>(T data) where T : class
        {
            string serializedJson = JsonConvert.SerializeObject(data);
            byte[] encodedData = Encoding.UTF8.GetBytes(serializedJson);
            string filePath = @$"{Name}.json";
            await File.WriteAllBytesAsync(filePath, encodedData);
        }
        
    }
}