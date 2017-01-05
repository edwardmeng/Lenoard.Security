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
        public string Key { get; private set; }

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
            var provider = context.RequestServices.GetService<ISiteMapProvider>();
            var siteMapNode = provider?.FindNode(Key);
            if (!string.IsNullOrEmpty(siteMapNode?.RequiredAction))
            {
                return Claims.GetPermissions(context).Contains(siteMapNode.RequiredAction, StringComparer.CurrentCultureIgnoreCase);
            }
            return true;
        }
    }
}
