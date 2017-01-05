using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    /// Used to declare the required permissions to a controller action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAttribute"/> class with the specified permissions.
        /// </summary>
        /// <param name="permissions">The specified permission keys.</param>
        public PermissionAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <summary>
        /// Gets the key to look up permissions.
        /// </summary>
        public string[] Permissions { get; }


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

        private bool AuthorizeCore(HttpContext context)
        {
            if (Permissions.Length <= 0) return true;
            var grantedPermissions = Claims.GetPermissions(context).ToArray();
            return Permissions.All(permission => grantedPermissions.Contains(permission));
        }
    }
}
