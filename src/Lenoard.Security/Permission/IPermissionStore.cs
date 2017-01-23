namespace Lenoard.Security
{
    /// <summary>
    /// Provides a common interface for all permission providers, and a way for developers 
    /// to implement custom permission providers. 
    /// </summary>
    public interface IPermissionStore
    {
        /// <summary>
        /// Gets the root <see cref="PermissionNode"/> collection of the action map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="PermissionNode"/> collection of the current permission provider. 
        /// </value>
        PermissionNodeCollection RootNodes { get; }
    }
}
