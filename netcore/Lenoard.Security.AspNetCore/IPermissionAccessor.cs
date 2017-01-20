using System.Collections.Generic;
using System.Security.Claims;

namespace Lenoard.Security.AspNetCore
{
    /// <summary>
    /// Provides methods to access the current user permissions. 
    /// </summary>
    public interface IPermissionAccessor
    {
        /// <summary>
        /// Determines whether the current user has the required permissions.
        /// </summary>
        /// <param name="permissions">The required permissions.</param>
        /// <returns><c>true</c> if the current user has all the required permissions; otherwise, <c>false</c>.</returns>
        bool HasPermissions(IEnumerable<string> permissions);

        /// <summary>
        /// Sets the granted permissions of the current user.
        /// </summary>
        /// <param name="permissions">The granted permissions.</param>
        void SetPermissions(IEnumerable<string> permissions);

        /// <summary>
        /// Sets the granted permissions of the specified claims principal.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to set permissions.</param>
        /// <param name="permissions">The granted permissions.</param>
        void SetPermissions(ClaimsPrincipal principal, IEnumerable<string> permissions);

        /// <summary>
        /// Clears the permissions of the current user.
        /// </summary>
        void ClearPermissions();
    }
}
