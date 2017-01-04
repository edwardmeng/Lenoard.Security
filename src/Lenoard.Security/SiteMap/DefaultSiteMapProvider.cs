namespace Lenoard.Security
{
    /// <summary>
    /// The default implementation of the <see cref="ISiteMapProvider"/> to persist <see cref="SiteMapNode"/>s
    /// in memory and the nodes can be manipulated programmatically.
    /// </summary>
    public class DefaultSiteMapProvider : ISiteMapProvider
    {
        /// <summary>
        /// Gets the root <see cref="SiteMapNode"/> collection of the site map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="SiteMapNode"/> collection of the current site map data provider. 
        /// </value>
        public SiteMapNodeCollection RootNodes { get; } = new SiteMapNodeCollection();
    }
}
