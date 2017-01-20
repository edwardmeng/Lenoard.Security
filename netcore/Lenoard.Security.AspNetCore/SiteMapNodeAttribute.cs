using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    /// Used to declare the mapping site map node to controller action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SiteMapNodeAttribute: ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeAttribute"/> class with the specified site map node key.
        /// </summary>
        /// <param name="key">The specified site map node lookup key.</param>
        public SiteMapNodeAttribute(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Gets the key to look up site map node.
        /// </summary>
        /// <value>
        /// The key to lookup site map node.
        /// </value>
        public string Key { get; }

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null && (actionDescriptor.MethodInfo.IsDefined<AllowAnonymousAttribute>() || actionDescriptor.ControllerTypeInfo.IsDefined<AllowAnonymousAttribute>()))
            {
                return;
            }
            if (!AuthorizeCore(context.HttpContext))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        /// <summary>
        /// When overridden, provides an entry point for custom permission checks.
        /// </summary>
        /// <param name="context">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns><c>true</c> if the current user has all the required permission; otherwise, <c>false</c>.</returns>
        protected virtual bool AuthorizeCore(HttpContext context)
        {
            var provider = context.RequestServices.GetService<ISiteMapProvider>();
            var requiredAction = provider?.FindNode(Key)?.RequiredPermission;
            if (string.IsNullOrEmpty(requiredAction)) return true;
            var permissionAccessor = context.RequestServices.GetRequiredService<IPermissionAccessor>();
            return permissionAccessor.HasPermissions(new[] {requiredAction});
        }
    }
}
