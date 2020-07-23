using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RocketToOpenMod.Model.Rocket.Permissions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RocketToOpenMod.Jobs
{
    public abstract class Job
    {
        public async virtual Task DoAsync()
        {
            
        }

        public Job(string name)
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