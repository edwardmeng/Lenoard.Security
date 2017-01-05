using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ServiceBridge;

namespace Lenoard.Security.Mvc
{
    public static class Claims
    {
        public const string PermissionType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/permissions";
        public const string PermissionContextKey = "lenoard/security/permissions";

        public static IEnumerable<string> GetPermissions(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var claimsPermissions = Enumerable.Empty<string>();
            if (context.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    claimsPermissions = from claim in claimsIdentity.Claims
                                        where claim.Type == PermissionType
                                        select claim.Value;
                }
            }
            var sessionPermissions = context.Session?[PermissionContextKey] as IEnumerable<string> ?? Enumerable.Empty<string>();
            return claimsPermissions.Union(sessionPermissions);
        }

        public static async Task LoadPermissionsToSessionAsync(HttpContextBase context, IEnumerable<string> roles)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }
            if (context.Session == null)
            {
                throw new InvalidOperationException("The session is not supported for this application or request.");
            }
            var permissions = new List<string>();
            var provider = ServiceContainer.GetInstance<IAuthenticateProvider>();
            foreach (var roleName in roles)
            {
                permissions.AddRange(await provider.GetRolePermissionsAsync(roleName, CancellationToken.None));
            }
            context.Session[PermissionContextKey] = permissions.Distinct().ToArray();
        }

        public static void UnloadPermissionsFromSession(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Session?.Remove(PermissionContextKey);
        }
    }
}
