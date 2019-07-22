using Dapper;
using PortalManagement.Models.Bussiness.Agency;
using PortalManagement.Models.Utilities.ConnectionString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.Agency
{
    public class AgencyDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static dynamic GetAgencies(int from, int to)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_From", from),
                    new SqlParameter("@_To", to)
                };
                helper.GetDataTableSP("SP_AgencyTransactionsSum", parsList.ToArray());
                return Convert.ToInt32(parsList[2].Value);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return -99;
        }

        public static dynamic GetAllAgencies()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT ID, Displayname, GameAccountId FROM [ag].[Account] WHERE Level != 0 AND IsLocked = 0 AND IsDelete = 0");
            }
        }

        public static dynamic GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT A.ID, A.Displayname, A.Tel, B.Gold, A.GameName, A.Level, A.Displayable, A.GameAccountId FROM [ag].[Account] A" +
                    " INNER JOIN [dbo].[Account] B ON A.GameAccountId = B.AccountID" +
                    " WHERE A.Level != 0 AND A.IsLocked = 0 AND A.IsDelete = 0" +
                    " ORDER BY A.Level ASC");
            }
        }

        public static dynamic Management(DateTime start, DateTime end)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT A.Displayname, COUNT(C.Code) Total, ISNULL(Sum(C.Gold), 0) TotalAmount FROM [GamePortal].[ag].[Account] A" +
                    " LEFT JOIN [dbo].[EventGiftcode] B ON A.ID = B.AgencyId" +
                    " LEFT JOIN [dbo].[Giftcode] C ON B.ID = C.EventID" +
                    $" WHERE B.CreatedTime >= '{start}' AND B.CreatedTime <= '{end}' AND A.Level != 0" +
                    " GROUP BY A.Displayname ORDER BY TotalAmount DESC");
            }
        }

        public static void DeleteAgency(long id)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"exec [ag].[DeleteAgency] @_id = {id}");
            }
        }

        public static void HideAgency(long id, int display)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var status = display > 0;

                connection.Execute($"UPDATE [ag].[Account] SET Displayable = '{display}' WHERE ID = {id}");
            }
        }

        public static void AddAgency(int creator, string creatorname, string display, string password, string tel, string fb, string tele, string address,
          string gameName, int level)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"exec [ag].[SP_CreateAgency] @_Displayname = N'{display}'," +
                    $"@_Username = '{gameName}'," +
                    $"@_Password = '{password}'," +
                    $"@_Tel = N'{tel}'," +
                    $"@_fb = N'{fb}'," +
                    $"@_Telegram = N'{tele}', " +
                    $"@_Information = N'{address}'," +
                    $"@_Creator = {creator}, " +
                    $"@_CreatorName = N'{creatorname}', " +
                    $"@_AgencyLevel = {level}");
            }
        }

        public static void AuthorizeAgency(long gameAccountId, int creator, string creatorname, string display, string password, string tel, string fb, string tele, string address,
         string gameName, int level)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                connection.Execute($"exec [dbo].[SP_AuthorizeAgency] @_Displayname = N'{display}'," +
                    $"@_GameAccountId = {gameAccountId}," +
                    $"@_GameName = {gameName}," +
                    $"@_Password = '{password}'," +
                    $"@_Tel = N'{tel}'," +
                    $"@_fb = N'{fb}'," +
                    $"@_Telegram = N'{tele}', " +
                    $"@_Information = N'{address}'," +
                    $"@_Creator = {creator}, " +
                    $"@_CreatorName = N'{creatorname}', " +
                    $"@_AgencyLevel = {level}");
            }
        }

        public static Bussiness.Agency.Agency GetById(int id)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            try
            {
                return helper.GetInstance<Bussiness.Agency.Agency>($"SELECT A.ID, A.GameAccountId, B.IsAgency FROM [ag].[Account] A " +
                    $"INNER JOIN [dbo].[Account] B ON A.GameAccountId = B.AccountID " +
                    $"WHERE A.ID = {id}");
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return null;
        }

        public static List<Ag> GetAgencies(int? id)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            var query = "select A.ID, A.GameAccountId, B.Gold, B.LockedGold, A.Displayname, A.Level FROM ag.Account A" +
                    " INNER JOIN [dbo].[Account] B ON A.GameAccountId = B.AccountID" +
                    " WHERE A.IsDelete = 0 AND Level != 0";
            if (id.HasValue)
                query += $" AND (A.ID = {id.Value} OR Creator = {id.Value})";
            return helper.GetList<Ag>(query);
        }

        public static List<AccountRate> GetAccountTransactionRate(long gameAccountId, int start, int end)
        {
            DBHelper helper = new DBHelper(_conStr);
            List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_AgenGameAccountId", gameAccountId),
                    new SqlParameter("@_From", start),
                    new SqlParameter("@_To", end)
                };

            return helper.GetListSP<AccountRate>("SP_GetAgencyReward", parsList.ToArray());
        }

        public static List<AccountRateDetail> GetAccountTransactionRateDetail(long gameAccountId, int start, int end)
        {
            DBHelper helper = new DBHelper(_conStr);
            List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_AgenGameAccountId", gameAccountId),
                    new SqlParameter("@_From", start),
                    new SqlParameter("@_To", end)
                };

            return helper.GetListSP<AccountRateDetail>("CMS_ListTransAccount", parsList.ToArray());
        }
    }
}