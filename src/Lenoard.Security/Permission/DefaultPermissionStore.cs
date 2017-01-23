namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of the <see cref="IPermissionStore"/> to persist <see cref="PermissionNode"/>s
    /// in memory and the nodes can be manipulated programmatically.
    /// </summary>
    public class DefaultPermissionStore : IPermissionStore
    {
        /// <summary>
        /// Gets the root <see cref="PermissionNode"/> collection of the action map data that the current store represents.
        /// </summary>
        /// <value>
        /// The root <see cref="PermissionNode"/> collection of the current permission store. 
        /// </value>
        public PermissionNodeCollection RootNodes { get; } = new PermissionNodeCollection();
    }
}
