namespace Lenoard.Security
{
    public class DefaultActionMapProvider : IActionMapProvider
    {
        public ActionMapNodeCollection RootNodes { get; } = new ActionMapNodeCollection();
    }
}
