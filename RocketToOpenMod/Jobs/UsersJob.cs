using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketToOpenMod.API;
using RocketToOpenMod.Data;
using RocketToOpenMod.Model.OpenMod.Users;
using RocketToOpenMod.Model.Rocket.Permissions;

namespace RocketToOpenMod.Jobs
{
    public class UsersJob : Job
    {
        public override async Task DoAsync()
        {
            await LogInfo("Loading user data from the following Rocket sources:" +
                              "\n" + "* Permissions");

            RocketPermissions rocketPermissions = await LoadRocketPermissionsAsync();

            if (rocketPermissions == null)
            {
                await LogInfo("Could not load Rocket permissions");
                return;
            }

            UsersData openMod = new UsersData {Users = new List<UserData>()};
            await LogInfo("Preparing user data");

            foreach (RocketPermissionsGroup group in rocketPermissions.Groups)
            {
                foreach (string user in group.Members)
                {
                    //TODO: Check what user id actually is
                    if (openMod.Users.All(u => u.Id != user))
                        openMod.Users.Add(new UserData()
                        {
                            Roles = new HashSet<string>(){group.Id},
                            Type = "player",
                            Data = new Dictionary<string, object>(),
                            FirstSeen = DateTime.Now,
                            LastSeen = DateTime.Today,
                            Id = user,
                            LastDisplayName = user,
                            Permissions = new HashSet<string>()
                        });
                    else if (openMod.Users.Any(u => u.Id == user))
                        openMod.Users.FirstOrDefault(u => u.Id == user)?.Roles.Add(group.Id);
                }
            }
            
            await LogInfo("Saving user data");
            await SaveAsync(openMod);

        }

        public UsersJob(WriteFileType write) : base(write, "users")
        {
        }
    }
}