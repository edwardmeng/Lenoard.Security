using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Lenoard.Security.AspNetCore
{
    internal static class Claims
    {
        public const string PermissionType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/permissions";

        public static IEnumerable<string> GetPermissions(HttpContext context)
        {
            return from identity in context.User.Identities
                where identity.IsAuthenticated
                from claim in identity.Claims
                where claim.Type == Claims.PermissionType
                select claim.Value;
        }
    }
}
