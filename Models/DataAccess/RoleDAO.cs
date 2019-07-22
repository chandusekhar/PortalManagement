using PortalManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using PortalManagement.Models.Utilities.ConnectionString;

namespace PortalManagement.Models.DataAccess
{
    public class RoleDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        
        public static IEnumerable<Api> GetApis()
        {
            using(var db = new SqlConnection(_conStr))
            {
                return db.Query<Api>("select * from dbo.Api");
            }
        }

        public static IEnumerable<Heading> GetHeadings()
        {
            using (var db = new SqlConnection(_conStr))
            {
                return db.Query<Heading>("select * from dbo.Heading ORDER BY IndexOrder");
            }
        }

        public static IEnumerable<SubHeading> GetSubHeadings()
        {
            using (var db = new SqlConnection(_conStr))
            {
                return db.Query<SubHeading>("select * from dbo.SubHeading");
            }
        }

        public static IEnumerable<Role> GetRoles()
        {
            using (var db = new SqlConnection(_conStr))
            {
                return db.Query<Role>("select * from dbo.Role");
            }
        }

        public static void Execute(string query)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                connection.Execute(query);
            }
        }
    }
}