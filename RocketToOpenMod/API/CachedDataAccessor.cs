using RocketToOpenMod.Model.OpenMod.Localisations;
using RocketToOpenMod.Model.OpenMod.Permissions;
using RocketToOpenMod.Model.OpenMod.Users;
using RocketToOpenMod.Model.Rocket.Permissions;
using RocketToOpenMod.Model.Rocket.Translations;

namespace RocketToOpenMod.API
{
    public class CachedDataAccessor
    {
        public PermissionRolesData OpenModPermissions { get; set; }
        public RocketPermissions RocketPermissions { get; set; }
        public UsersData OpenModUsers { get; set; }
        public commands OpenModLocalisation { get; set; }
        public TranslationList RocketTranslations { get; set; }
    }
}