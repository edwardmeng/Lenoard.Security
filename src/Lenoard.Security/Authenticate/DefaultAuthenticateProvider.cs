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
        /// Asynchronously authorizes actions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="actionKeys">The action keys.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual Task AuthorizeRoleAsync(string roleName, string[] actionKeys, CancellationToken cancellationToken)
        {
            AuthorizeRole(roleName, actionKeys);
            return Task.Delay(0, cancellationToken);
        }

        /// <summary>
        /// Authorizes actions to role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="actionKeys">The action keys.</param>
        protected virtual void AuthorizeRole(string roleName, string[] actionKeys)
        {
            _roleActions.AddOrUpdate(roleName, actionKeys ?? new string[0]);
        }

        /// <summary>
        /// Asynchronously gets the role authorized actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the authorized action keys.</returns>
        public virtual Task<string[]> GetRoleAuthorizedActionsAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetRoleAuthorizedActions(roleName));
        }

        /// <summary>
        /// Gets the role authorized actions.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The authorized action keys.</returns>
        protected virtual string[] GetRoleAuthorizedActions(string roleName)
        {
            string[] actions;
            return _roleActions.TryGetValue(roleName, out actions) ? actions : new string[0];
        }
    }
}
