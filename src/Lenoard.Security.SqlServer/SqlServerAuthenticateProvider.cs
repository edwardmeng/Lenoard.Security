﻿using System;
using System.Threading.Tasks;

namespace Lenoard.Security.SqlServer
{
    public class SqlServerAuthenticateProvider : IAuthenticateProvider
    {
        public Task AuthorizeRoleAsync(string roleName, string[] permissions)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetRolePermissionsAsync(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
