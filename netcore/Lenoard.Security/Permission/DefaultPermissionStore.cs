namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of the <see cref="IPermissionProvider"/> to persist <see cref="PermissionNode"/>s
    /// in memory and the nodes can be manipulated programmatically.
    /// </summary>
    public class DefaultPermissionProvider : IPermissionProvider
    {
        /// <summary>
        /// Gets the root <see cref="PermissionNode"/> collection of the action map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="PermissionNode"/> collection of the current permission provider. 
        /// </value>
        public PermissionNodeCollection RootNodes { get; } = new PermissionNodeCollection();
    }
}
