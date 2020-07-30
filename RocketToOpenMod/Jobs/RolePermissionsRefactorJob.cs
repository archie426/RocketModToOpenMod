using System;
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
                Console.WriteLine("[~] Could not load OpenMod translations!");
                return;
            }
            
            foreach (PermissionRoleData role in openMod.Roles)
            {
                foreach (string perm in role.Permissions.Where(p => p.Contains("unturned.")))
                {
                    role.Permissions.Remove(perm);
                    role.Permissions.Add(perm.Replace("unturned.", "OpenMod.commands.unturned"));
                }

                foreach (string perm in role.Permissions.Where(p => Conversions.ContainsKey(p)))
                {
                    role.Permissions.Remove(perm);
                    role.Permissions.Add(Conversions[perm]);
                }
            }

            await SaveAsync(openMod);

        }
    }
}