using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of <see cref="IAuthenticateProvider"/> to persist role permission in memory.
    /// </summary>
    public class DefaultAuthenticateProvider : IAuthenticateProvider
    {
        private readonly IDictionary<string, string[]> _roleActions = new Dictionary<string, string[]>();

        /// <summary>
        /// Asynchronously grants permissions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="permissions">The permissions to be granted.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual Task AuthorizeRoleAsync(string roleName, string[] permissions, CancellationToken cancellationToken)
        {
            AuthorizeRole(roleName, permissions);
            return Task.Delay(0, cancellationToken);
        }

        /// <summary>
        /// Grants permissions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="permissions">The permissions to be granted.</param>
        protected virtual void AuthorizeRole(string roleName, string[] permissions)
        {
            _roleActions.AddOrUpdate(roleName, permissions ?? new string[0]);
        }

        /// <summary>
        /// Asynchronously retrieves the role granted actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the granted permissions.</returns>
        public virtual Task<string[]> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetRolePermissions(roleName));
        }

        /// <summary>
        /// Retrieves the role granted actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The granted permissions.</returns>
        protected virtual string[] GetRolePermissions(string roleName)
        {
            string[] actions;
            return _roleActions.TryGetValue(roleName, out actions) ? actions : new string[0];
        }
    }
}
