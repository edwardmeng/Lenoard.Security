namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of the <see cref="IActionMapProvider"/> to persist <see cref="ActionMapNode"/>s
    /// in memory and the nodes can be manipulated programmatically.
    /// </summary>
    public class DefaultActionMapProvider : IActionMapProvider
    {
        /// <summary>
        /// Gets the root <see cref="ActionMapNode"/> collection of the action map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="ActionMapNode"/> collection of the current action map data provider. 
        /// </value>
        public ActionMapNodeCollection RootNodes { get; } = new ActionMapNodeCollection();
    }
}
