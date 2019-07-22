using PortalManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;
using PortalManagement.Models.Bussiness;

namespace PortalManagement.Models.DataAccess
{
    public class DashboardDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static List<Dashboard> GetDashboard(int start, int end)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_From", start),
                    new SqlParameter("@_To", end)
                };
                return helper.GetListSP<Dashboard>("SP_Dashboard", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<Dashboard>();
        }
    }
}