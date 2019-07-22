using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Utilities.ConnectionString;

namespace PortalManagement.Models.DataAccess.Authentication
{
    public class AccountDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        
        public static dynamic Authenticate(string username, string password)
        {
            using(var connection = new SqlConnection(_conStr))
            {
                return connection.QueryFirstOrDefault($"select AccountID, AccountName, GroupID from dbo.Account WHERE AccountName = '{username}' AND " +
                    $"Password = '{password}'");
            }
        }
    }
}