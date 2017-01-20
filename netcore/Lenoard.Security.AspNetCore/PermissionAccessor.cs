using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Lenoard.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    /// Provides methods to access the current user permissions. 
    /// </summary>
    public class PermissionAccessor : IPermissionAccessor
    {
        private const string PermissionType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/permissions";
        private const string PermissionContextKey = "lenoard/security/permissions";
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAccessor"/> class. 
        /// </summary>
        /// <param name="httpContextAccessor">The accessor to <see cref="HttpContext"/>.</param>
        public PermissionAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public bool HasPermissions(IEnumerable<string> permissions)
        {
            var context = _httpContextAccessor.HttpContext;
            string[] sessionPermissions = null;
            return permissions.All(permission => context.User.Identities.Any(identity => identity.IsAuthenticated && identity.HasClaim(PermissionType, permission)) || (sessionPermissions ?? (sessionPermissions = GetSessionPermissions())).Contains(permission));
        }

        /// <inheritdoc />
        public void SetPermissions(IEnumerable<string> permissions)
        {
            ClearPermissions();
            if (permissions == null) return;
            var context = _httpContextAccessor.HttpContext;
            var identity = context.User.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if (identity != null)
            {
                identity.AddClaims(permissions.Select(permission => new Claim(PermissionType, permission)));
            }
            else
            {
                var feature = context.Features.Get<ISessionFeature>();
                if (feature?.Session?.IsAvailable ?? false)
                    feature.Session.SetString(PermissionContextKey, string.Join(";", permissions));
            }
        }

        /// <inheritdoc />
        public void ClearPermissions()
        {
            var context = _httpContextAccessor.HttpContext;
            foreach (var identity in context.User.Identities)
                identity.RemoveClaims(PermissionType);
            var feature = context.Features.Get<ISessionFeature>();
            if (feature?.Session?.IsAvailable ?? false)
                feature.Session.Remove(PermissionContextKey);
        }

        private string[] GetSessionPermissions()
        {
            string[] permissions = null;
            var feature = _httpContextAccessor.HttpContext.Features.Get<ISessionFeature>();
            if (feature?.Session?.IsAvailable ?? false)
                permissions = feature.Session.GetString(PermissionContextKey)?
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (permissions == null)
                permissions = new string[0];
            return permissions;
        }

        /// <inheritdoc />
        public void SetPermissions(ClaimsPrincipal principal, IEnumerable<string> permissions)
        {
            foreach (var identity in principal.Identities)
                identity.RemoveClaims(PermissionType);
            principal.Identities.FirstOrDefault(x => x.IsAuthenticated)?.AddClaims(permissions.Select(permission => new Claim(PermissionType, permission)));
        }
    }
}