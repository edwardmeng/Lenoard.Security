namespace Lenoard.Security.SqlServer
{
    public class SqlServerPermissionStore : IPermissionStore
    {
        public PermissionNodeCollection RootNodes { get; }
    }
}
