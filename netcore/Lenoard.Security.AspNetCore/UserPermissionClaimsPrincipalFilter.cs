using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lenoard.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    /// Provides claims principal filter to configure user permission for the authenticated principal.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserPermissionClaimsPrincipalFilter<TUser> : IUserClaimsPrincipalFilter<TUser>
        where TUser : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionClaimsPrincipalFilter{TUser}"/> class. 
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{TUser}"/> to retrieve user information from.</param>
        /// <param name="authenticateProvider">The <see cref="IAuthenticateProvider"/> to retrieve user permission from.</param>
        /// <param name="optionsAccessor">The configured <see cref="IdentityOptions"/>.</param>
        /// <param name="permissionAccessor">The accesser of user permissions.</param>
        public UserPermissionClaimsPrincipalFilter(UserManager<TUser> userManager, IAuthenticateProvider authenticateProvider, IOptions<IdentityOptions> optionsAccessor, IPermissionAccessor permissionAccessor)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));
            if (authenticateProvider == null)
                throw new ArgumentNullException(nameof(authenticateProvider));
            if (permissionAccessor == null)
                throw new ArgumentNullException(nameof(permissionAccessor));
            UserManager = userManager;
            AuthenticateProvider = authenticateProvider;
            Options = optionsAccessor?.Value ?? new IdentityOptions();
            PermissionAccessor = permissionAccessor;
        }

        /// <summary>
        /// Get the <see cref="UserManager{TUser}"/> to retrieve user information from.
        /// </summary>
        protected internal UserManager<TUser> UserManager { get; }

        /// <summary>
        /// Get the <see cref="IAuthenticateProvider"/> to retrieve user permission from.
        /// </summary>
        protected internal IAuthenticateProvider AuthenticateProvider { get; }

        /// <summary>
        /// Gets the configured <see cref="IdentityOptions"/>.
        /// </summary>
        protected internal IdentityOptions Options { get; set; }

        /// <summary>
        /// Gets the accesser of user permissions.
        /// </summary>
        protected internal IPermissionAccessor PermissionAccessor { get; }

        /// <summary>
        /// Configures user permission for the authenticated principal.
        /// </summary>
        /// <param name="user">The user to configure a <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="principal">The principal to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task ConfigureAsync(TUser user, ClaimsPrincipal principal)
        {
            if (UserManager.SupportsUserRole)
            {
                var roleNames = from identity in principal.Identities
                                where identity.IsAuthenticated
                                from claim in identity.Claims
                                where claim.Type == Options.ClaimsIdentity.RoleClaimType
                                select claim.Value;
                var permissions = new List<string>();
                foreach (var roleName in roleNames)
                {
                    permissions.AddRange(await AuthenticateProvider.GetRolePermissionsAsync(roleName));
                }
                PermissionAccessor.SetPermissions(principal, permissions.Distinct());
            }
        }
    }
}