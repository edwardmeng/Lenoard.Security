using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lenoard.Security.AspNetCore
{
    internal class UserPermissionClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
        where TUser : class
        where TRole : class
    {

        public IAuthenticateProvider AuthenticateProvider { get; }

        public UserPermissionClaimsPrincipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IAuthenticateProvider authenticateProvider)
            : base(userManager, roleManager, optionsAccessor)
        {
            if (authenticateProvider == null)
            {
                throw new ArgumentNullException(nameof(authenticateProvider));
            }
            AuthenticateProvider = authenticateProvider;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principle = await base.CreateAsync(user);
            if (UserManager.SupportsUserRole)
            {
                var actions = new List<string>();
                foreach (var claim in principle.Claims)
                {
                    if (claim.Type == Options.ClaimsIdentity.RoleClaimType)
                    {
                        actions.AddRange(await AuthenticateProvider.GetRolePermissionsAsync(claim.Value, CancellationToken.None));
                    }
                }
                var identity = principle.Identities.First(x => x.IsAuthenticated);
                foreach (var action in actions.Distinct())
                {
                    identity.AddClaim(new Claim(Claims.PermissionType, action));
                }
            }
            return principle;
        }
    }
}
