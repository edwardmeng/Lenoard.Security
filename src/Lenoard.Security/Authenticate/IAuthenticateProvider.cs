using System.Threading.Tasks;

namespace Lenoard.Security
{
    /// <summary>
    /// Represents the common interface to defined the methods of 
    /// authorizing role permimissions and retrieving the authorized permissions of the specified role.
    /// </summary>
    public interface IAuthenticateProvider
    {
        /// <summary>
        /// Asynchronously grants permissions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="permissions">The permissions to be granted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AuthorizeRoleAsync(string roleName, string[] permissions);

        /// <summary>
        /// Asynchronously retrieves the role granted actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the granted permissions.</returns>
        Task<string[]> GetRolePermissionsAsync(string roleName);
    }
}
