using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketToOpenMod.Model.OpenMod.Permissions;
using RocketToOpenMod.Model.Rocket.Permissions;

namespace RocketToOpenMod.Jobs
{
    public class PermissionsJob : Job
    {
        
        
        public override async Task DoAsync()
        {
            RocketPermissions rocket = await LoadRocketPermissionsAsync();

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
                Id = group.Id,
                Parents = new HashSet<string>(),
                Priority = group.Priority,
                Permissions = new HashSet<string>(),
                Data = new Dictionary<string, object>(),
                DisplayName = group.DisplayName,
                IsAutoAssigned = false
            };

            foreach (Permission rocketPerm in group.Permissions)
                data.Permissions.Add(rocketPerm.Name);
            
            return data;

        }

        public PermissionsJob() : base( "permissions")
        {
        }
    }
}