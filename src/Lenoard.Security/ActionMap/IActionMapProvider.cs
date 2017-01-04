namespace Lenoard.Security
{
    /// <summary>
    /// Provides a common interface for all action map data providers, and a way for developers 
    /// to implement custom action map data providers that can be used with the ASP.NET action map infrastructure. 
    /// </summary>
    public interface IActionMapProvider
    {
        /// <summary>
        /// Gets the root <see cref="ActionMapNode"/> collection of the action map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="ActionMapNode"/> collection of the current action map data provider. 
        /// </value>
        ActionMapNodeCollection RootNodes { get; }
    }
}
