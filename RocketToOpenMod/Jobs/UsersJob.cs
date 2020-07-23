using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketToOpenMod.Model.OpenMod.Users;
using RocketToOpenMod.Model.Rocket.Permissions;

namespace RocketToOpenMod.Jobs
{
    public class UsersJob : Job
    {
        public override async Task Do()
        {
            Console.WriteLine("[~] Loading user data from the following Rocket sources:" +
                              "\n" + "* Permissions");

            RocketPermissions rocketPermissions = await LoadRocketPermissionsAsync();

            UsersData openMod = new UsersData {Users = new List<UserData>()};
            
            Console.WriteLine("[~] Preparing user data");

            foreach (RocketPermissionsGroup group in rocketPermissions.Groups)
            {
                foreach (string user in group.Members)
                {
                    //TODO: Check what user id actually is
                    if (openMod.Users.All(u => u.Id != user))
                        openMod.Users.Add(new UserData()
                        {
                            Roles = new HashSet<string>(){group.Id},
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
            
            Console.WriteLine("[~] Saving user data");
            await SaveAsync(openMod);

        }

        public UsersJob() : base("users")
        {
        }
    }
}