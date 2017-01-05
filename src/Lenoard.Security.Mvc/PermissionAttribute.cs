using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lenoard.Security.Mvc
{
    /// <summary>
    /// Used to declare the required permissions to a controller action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute: ActionFilterAttribute, IAuthorizationFilter
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

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context, which encapsulates information for using <see cref="PermissionAttribute"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="filterContext"/> parameter is null.
        /// </exception>
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("PermissionAttribute cannot be used within a child action caching block.");
            }
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                if (AuthorizeCore(filterContext.HttpContext))
                {
                    var cache = filterContext.HttpContext.Response.Cache;
                    cache.SetProxyMaxAge(new TimeSpan(0L));
                    cache.AddValidationCallback(CacheValidateHandler, null);
                }
                else
                {
                    HandleUnauthorizedRequest(filterContext);
                }
            }
        }

        protected virtual bool Authenticate(string[] grantedPermissions)
        {
            return Permissions.All(permission => grantedPermissions.Contains(permission, StringComparer.CurrentCultureIgnoreCase));
        }

        private bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            if (Permissions.Length <= 0) return true;
            return Authenticate(Claims.GetPermissions(httpContext).ToArray());
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        private HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            return !AuthorizeCore(httpContext) ? HttpValidationStatus.IgnoreThisRequest : HttpValidationStatus.Valid;
        }
    }
}
