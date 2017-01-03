using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lenoard.Security
{
    public class DefaultAuthenticateProvider : IAuthenticateProvider
    {
        private readonly IDictionary<string, string[]> _roleActions = new Dictionary<string, string[]>();

        public virtual Task AuthorizeRoleAsync(string roleName, string[] actionKeys, CancellationToken cancellationToken)
        {
            AuthorizeRole(roleName, actionKeys);
            return Task.Delay(0, cancellationToken);
        }

        protected virtual void AuthorizeRole(string roleName, string[] actionKeys)
        {
            _roleActions.AddOrUpdate(roleName, actionKeys ?? new string[0]);
        }

        public virtual Task<string[]> GetRoleAuthorizedActionsAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetRoleAuthorizedActions(roleName));
        }

        protected virtual string[] GetRoleAuthorizedActions(string roleName)
        {
            string[] actions;
            return _roleActions.TryGetValue(roleName, out actions) ? actions : new string[0];
        }
    }
}
