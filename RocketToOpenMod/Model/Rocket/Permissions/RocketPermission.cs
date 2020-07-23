using System;
using System.Xml.Serialization;

namespace RocketToOpenMod.Model.Rocket.Permissions
{
    [Serializable]
    public class Permission
    {
        [XmlAttribute]
        public uint Cooldown = 0;

        [XmlText]
        public string Name = "";

        public Permission() { }

        public Permission(string name, uint cooldown = 0)
        {
            Name = name;
            Cooldown = cooldown;
        }
        
    }
}