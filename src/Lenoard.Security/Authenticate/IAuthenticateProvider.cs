using System.Threading;
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
        /// Asynchronously authorizes actions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="actionKeys">The action keys.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AuthorizeRoleAsync(string roleName, string[] actionKeys, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets the role authorized actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the authorized action keys.</returns>
        Task<string[]> GetRoleAuthorizedActionsAsync(string roleName, CancellationToken cancellationToken);
    }
}
