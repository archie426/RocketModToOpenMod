using System;

namespace RocketToOpenMod.API
{
    public class ExternalJobAttribute : Attribute
    {
        public ExternalJobAttribute(string name)
        {
            Name = name;
        }

        [Obsolete("Please add a name")]
        public ExternalJobAttribute()
        {
            Name = "undefined";
        }

        public string Name { get; }
        
        
        
    }
}