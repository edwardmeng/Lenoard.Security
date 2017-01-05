using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using ServiceBridge;
using System.Threading;

namespace Lenoard.Security.AspNetCore
{
    public static class Claims
    {
        public const string PermissionType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/permissions";
        public const string PermissionContextKey = "lenoard/security/permissions";

        public static IEnumerable<string> GetPermissions(HttpContext context)
        {
            var claimsPermissions = from identity in context.User.Identities
                                    where identity.IsAuthenticated
                                    from claim in identity.Claims
                                    where claim.Type == PermissionType
                                    select claim.Value;
            var sessionPermissions = Enumerable.Empty<string>();
            var feature = context.Features.Get<ISessionFeature>();
            if (feature?.Session?.IsAvailable ?? false)
            {
                sessionPermissions = feature.Session.GetString(PermissionContextKey)?
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));
            }
            return claimsPermissions.Union(sessionPermissions);
        }

        public static async Task LoadPermissionsToSessionAsync(HttpContext context, IEnumerable<string> roles)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }
            var session = context.Session;
            var permissions = new List<string>();
            var provider = ServiceContainer.GetInstance<IAuthenticateProvider>();
            foreach (var roleName in roles)
            {
                permissions.AddRange(await provider.GetRolePermissionsAsync(roleName, CancellationToken.None));
            }
            session.SetString(PermissionContextKey, permissions.Distinct().Join(","));
        }

        public static void UnloadPermissionsFromSession(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Session.Remove(PermissionContextKey);
        }
    }
}
