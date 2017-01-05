using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBridge;

namespace Lenoard.Security.Mvc
{
    /// <summary>
    /// Used to declare the mapping site map node to an ASP.NET page or controller action.
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

        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("SiteMapNodeAttribute cannot be used within a child action caching block.");
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

        protected virtual bool Authenticate(string[] grantedPermissions, string requiredPermission)
        {
            return grantedPermissions.Contains(requiredPermission, StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">
        /// The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.
        /// </param>
        /// <returns><c>true</c> if the user is authorized; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="httpContext"/> parameter is null.
        /// </exception>
        private bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            var provider = ServiceContainer.GetInstance<ISiteMapProvider>();
            var requiredAction = provider?.FindNode(Key)?.RequiredPermission;
            return string.IsNullOrEmpty(requiredAction) || Authenticate(Claims.GetPermissions(httpContext).ToArray(), requiredAction);
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">
        /// Encapsulates the information for using <see cref="AuthorizeAttribute"/>. 
        /// The <paramref name="filterContext"/> object contains the controller, HTTP context, request context, action result, and route data.
        /// </param>
        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        /// <summary>
        /// Called when the caching module requests authorization.
        /// </summary>
        /// <param name="httpContext">
        /// The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.
        /// </param>
        /// <returns>A reference to the validation status.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="httpContext"/> parameter is null.
        /// </exception>
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
