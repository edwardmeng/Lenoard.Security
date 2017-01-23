namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of the <see cref="ISiteMapStore"/> to persist <see cref="SiteMapNode"/>s
    /// in memory and the nodes can be manipulated programmatically.
    /// </summary>
    public class DefaultSiteMapStore : ISiteMapStore
    {
        /// <summary>
        /// Gets the root <see cref="SiteMapNode"/> collection of the site map data that the current store represents.
        /// </summary>
        /// <value>
        /// The root <see cref="SiteMapNode"/> collection of the current site map data store. 
        /// </value>
        public SiteMapNodeCollection RootNodes { get; } = new SiteMapNodeCollection();
    }
}
