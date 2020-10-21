using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketToOpenMod.API;
using RocketToOpenMod.Data;
using RocketToOpenMod.Model.OpenMod.Permissions;

namespace RocketToOpenMod.Jobs
{
    public class RolePermissionsRefactorJob : Job
    {
        
        private static readonly Dictionary<string, string> Conversions = new Dictionary<string, string>()
        {
            {"rocket.admin", "OpenMod.Unturned.commands.admin"},
            {"rocket.effect", "OpenMod.Unturned.commands.effect"},
            {"rocket.unadmin", "OpenMod.Unturned.commands.unadmin"},
        };

        public RolePermissionsRefactorJob(WriteFileType write) : base(write, "permissions")
        {
            
        }
        

        public override async Task DoAsync()
        {

            PermissionRolesData openMod = await LoadOpenPermissionsAsync();
            
            if (openMod == null)
            {
                await LogInfo("Could not load OpenMod permissions!");
                return;
            }

            await LogInfo("Preparing OpenMod permissions");
            
            foreach (PermissionRoleData role in openMod.Roles)
            {
                foreach (string perm in role.Permissions.ToList().Where(p => p.Contains("unturned.")))
                {
                    role.Permissions.Remove(perm);
                    role.Permissions.Add(perm.Replace("unturned.", "OpenMod.commands.unturned"));
                }

                foreach (string perm in role.Permissions.ToList().Where(p => Conversions.ContainsKey(p)))
                {
                    role.Permissions.Remove(perm);
                    role.Permissions.Add(Conversions[perm]);
                }

                foreach (string perm in role.Permissions.ToList().Where(p =>
                    !Conversions.ContainsKey(p) && !p.Contains("unturned.")))
                {
                    role.Permissions.Remove(perm);
                    role.Permissions.Add("Rocket.PermissionLink:" + perm);
                }
                
            }
            
            await LogInfo("Saving OpenMod permissions");

            await SaveAsync(openMod);

        }
    }
}