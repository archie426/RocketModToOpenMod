using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketToOpenMod.API;
using RocketToOpenMod.Data;
using RocketToOpenMod.Model.OpenMod.Permissions;
using RocketToOpenMod.Model.Rocket.Permissions;

namespace RocketToOpenMod.Jobs
{
    public class RolePermissionsReformatJob : Job
    {
        protected override async Task DoAsync()
        {
            RocketPermissions rocket = await LoadRocketPermissionsAsync();

            if (rocket == null)
            {
                await LogInfo("Could not load Rocket permissions!");
                return;
            }
            
            await LogInfo("Preparing OpenMod permissions");

            PermissionRolesData openMod = new PermissionRolesData {Roles = new List<PermissionRoleData>()};
            
            foreach (RocketPermissionsGroup group in rocket.Groups)
                openMod.Roles.Add(await GetRoleFromRocketGroup(group));
            
            await LogInfo("Saving OpenMod permissions");

            await SaveAsync(openMod);

        }


        public RolePermissionsReformatJob(WriteFileType write) : base(write, "permissions")
        {
            
        }
    }
}