using System;
using Lenoard.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    ///     Contains extension methods to <see cref="IdentityBuilder" /> for configuring identity services.
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adds the default implementations for user permission support.
        /// </summary>
        /// <param name="builder">The <see cref="IdentityBuilder" /> configuration with.</param>
        /// <returns>The <see cref="IdentityBuilder" /> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IdentityBuilder AddPermissionSupport(this IdentityBuilder builder)
        {
            builder.Services.TryAddSingleton<ISiteMapProvider, DefaultSiteMapProvider>();
            builder.Services.TryAddSingleton<IPermissionProvider, DefaultPermissionProvider>();
            builder.Services.TryAddSingleton<IAuthenticateProvider, DefaultAuthenticateProvider>();
            builder.Services.TryAddSingleton<IPermissionAccessor, PermissionAccessor>();
            builder.Services.AddScoped(typeof(IUserClaimsPrincipalFilter<>).MakeGenericType(builder.UserType), typeof(UserPermissionClaimsPrincipalFilter<>).MakeGenericType(builder.UserType));
            return builder;
        }
    }
}
