namespace Lenoard.Security
{
    public class DefaultSiteMapProvider : ISiteMapProvider
    {
        public SiteMapNodeCollection RootNodes { get; } = new SiteMapNodeCollection();
    }
}
