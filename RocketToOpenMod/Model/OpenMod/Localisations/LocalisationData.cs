using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace RocketToOpenMod.Model.OpenMod.Localisations
{
    //Excuse the naming case but I have no idea how this works lol
    

    [Serializable]
    public class commands
    {
        public Dictionary<string, string> openmod { get; set; }
        public Dictionary<string, string> errors { get; set; }

        public commands()
        {
            openmod = new Dictionary<string, string>();
            errors = new Dictionary<string, string>();
        }

        public commands(Dictionary<string, string> open, Dictionary<string, string> error)
        {
            openmod = open;
            errors = error;
        }
        
    }
    
}