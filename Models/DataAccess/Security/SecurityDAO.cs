using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Utilities.ConnectionString;

namespace PortalManagement.Models.DataAccess.Security
{
    public class SecurityDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        
        public static IEnumerable<dynamic> GetAccount()
        {
            using (var db = new SqlConnection(_conStr))
            {
                return db.Query("select AccountID, AccountName from [dbo].[Account] where GroupID != 1");
            }
        }

        public static void AddAccount(string username, string password, int userId)
        {
            using (var db = new SqlConnection(_conStr))
            {
                db.Execute("insert into dbo.account (AccountName, Password, GroupID, CreatedTime, CreatedBy) " +
                    $"values ('{username}', '{password}', 2, getdate(), {userId})");
            }
        }

        public static void DeleteAccount(int userId)
        {
            using (var db = new SqlConnection(_conStr))
            {
                db.Execute($"delete from dbo.account where accountid = {userId}; delete from dbo.Role where AccountID = {userId}");
            }
        }
    }
}