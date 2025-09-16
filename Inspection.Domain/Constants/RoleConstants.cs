using Inspection.Domain.Enum;

namespace Inspection.Domain.Constants
{
    public static class RoleConstants
    {
        public const string Admin = nameof(UserRole.Admin);
        public const string Inspector = nameof(UserRole.Inspector);

        public static class Names
        {
            public const string Admin = "Admin";
            public const string Inspector = "Inspector";
        }

        public static bool IsInspectorRole(string roleName)
        {
            return string.Equals(roleName, Names.Inspector, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAdminRole(string roleName)
        {
            return string.Equals(roleName, Names.Admin, StringComparison.OrdinalIgnoreCase);
        }
    }
}
