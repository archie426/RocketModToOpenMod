using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RocketToOpenMod.Model.Rocket.Permissions
{
    [Serializable]
    public class RocketPermissions
    {
        public RocketPermissions()
        {
        }

        [XmlElement("DefaultGroup")]
        public string DefaultGroup = "default";

        [XmlArray("Groups")]
        [XmlArrayItem(ElementName = "Group")]
        public List<RocketPermissionsGroup> Groups = new List<RocketPermissionsGroup>();
        
    }
}