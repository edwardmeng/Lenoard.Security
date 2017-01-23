namespace Lenoard.Security
{
    /// <summary>
    /// Provides a common interface for all site map data providers, and a way for developers 
    /// to implement custom site map data providers that can be used with the ASP.NET site map infrastructure 
    /// </summary>
    public interface ISiteMapStore
    {
        /// <summary>
        /// Gets the root <see cref="SiteMapNode"/> collection of the site map data that the current provider represents.
        /// </summary>
        /// <value>
        /// The root <see cref="SiteMapNode"/> collection of the current site map data provider. 
        /// </value>
        SiteMapNodeCollection RootNodes { get; }
    }
}
