using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ServiceBridge;

namespace Lenoard.Security.Mvc
{
    public class PermissionClaimsIdentityFactory<TUser, TKey>:ClaimsIdentityFactory<TUser,TKey>
        where TUser:class, IUser<TKey>
        where TKey:IEquatable<TKey>
    {
        public override async Task<ClaimsIdentity> CreateAsync(UserManager<TUser, TKey> manager, TUser user, string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);
            if (identity.IsAuthenticated && manager.SupportsUserRole)
            {
                var provider = ServiceContainer.GetInstance<IAuthenticateProvider>();
                var permissions = new List<string>();
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type == RoleClaimType)
                    {
                        permissions.AddRange(await provider.GetRolePermissionsAsync(claim.Value, CancellationToken.None));
                    }
                }
                foreach (var permission in permissions.Distinct())
                {
                    identity.AddClaim(new Claim(Claims.PermissionType, permission));
                }
            }
            return identity;
        }
    }

    public class PermissionClaimsIdentityFactory<TUser> : PermissionClaimsIdentityFactory<TUser, string>
        where TUser : class, IUser<string>
    {
    } 
}
