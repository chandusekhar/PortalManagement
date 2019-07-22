using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Bussiness.UserInfo;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.UserInfo
{
    public class UserDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static dynamic ExecuteQuery(string query)
        {
            using(var connect = new SqlConnection(_conStrPortalDbRoot))
            {
                return connect.Query(query);
            }
        }

        public static dynamic GetLoginLog(long accountId, int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                Console.WriteLine($"select * from [log].[Login] where AccountID = {accountId} order by id desc offset {skip} rows fetch next {itemPerpage} rows only");
                return connection.Query($"select * from [log].[Login] where AccountID = {accountId} order by id desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static void OffLoginOTP(long accountId)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update dbo.Account set IsOTP = 0 where AccountID = {accountId}");
            }
        }

        public static void BlockLogin(long accountId, string reason, int status, int blocker)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update dbo.Account set IsBlocked = {status} where AccountID = {accountId}");
            }

            using (var connection = new SqlConnection(_conStr))
            {
                connection.Execute($"exec SP_BlockLogin @_AccountID = {accountId}, @_Reason = N'{reason}', @_Status = {status}, @_Author = {blocker}");
            }
        }

        public static void BlockChat(long accountId, int status, int blocker)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update dbo.Account set IsMute = {status} where AccountID = {accountId}");
            }

            using (var connection = new SqlConnection(_conStr))
            {
                connection.Execute($"exec SP_BlockChat @_AccountID = {accountId}, @_Status = {status}, @_Author = {blocker}");
            }
        }

        public static void ChangePassword(long accountId, string password)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update dbo.Account set Password = '{password}' where AccountID = {accountId}");
            }
        }

        public static dynamic GetListBlockLogin(int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                return connection.Query($"select A.*, B.DisplayName, C.AccountName, B.Gold, A.Reason from [dbo].[Banned] A " +
                    $"inner join [GamePortal].[dbo].Account B on A.AccountID = B.AccountID " +
                    $"inner join dbo.Account C ON A.AuthorBlockLogin = C.AccountID" +
                    $" where A.LoginStatus = 1 " +
                    $" order by A.createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetListBlockChat(int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                return connection.Query($"select A.*, B.DisplayName, C.AccountName from [dbo].[Banned] A " +
                    $"inner join [GamePortal].dbo.Account B on A.AccountID = B.AccountID " +
                    $"inner join dbo.Account C ON A.AuthorBlockChat = C.AccountID" +
                    $" where A.ChatStatus = 1 " +
                    $" order by A.createdtimechat desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static List<NRUItem> GetNRU(DateTime start, DateTime end)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            try
            {
                var query = "SELECT COUNT(*) TotalAccount, DeviceType, CONVERT(Date, CreatedTime) CreatedDate FROM [GamePortal].[log].[Login] WITH (NOLOCK)" +
                    $" WHERE IsRegister = 1 AND CONVERT(Date, CreatedTime) >= '{start}' AND CONVERT(Date, CreatedTime) <= '{end}'" +
                    $" GROUP BY DeviceType, CONVERT(Date, CreatedTime)" +
                    $" ORDER BY CreatedDate DESC";
                return helper.GetList<NRUItem>(query);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<NRUItem>();
        }

        public static List<NRUItem> GetDAU(int start, int end)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            try
            {
                var query = "SELECT CONVERT(DATE, CreatedTime) CreatedDate, COUNT(DISTINCT AccountID) TotalAccount, DeviceType FROM [log].[Login] WITH (NOLOCK)" +
                    $" WHERE CreatedTime >= '{start}' AND CreatedTime <= '{end}' AND IsRegister = 0" +
                    $" GROUP BY CONVERT(DATE, CreatedTime), DeviceType" +
                    $" ORDER BY CONVERT(DATE, CreatedTime) DESC";
                return helper.GetList<NRUItem>(query);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<NRUItem>();
        }

        public static dynamic GetPU(int start, int end)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"SELECT CONVERT(Date, CreatedTime) Date, COUNT(DISTINCT AccountID) Total FROM [GamePortal].[log].[TopupCard] WITH (NOLOCK) WHERE CreatedTimeInt >= {start} AND CreatedTimeInt <= {end}" +
                    $" GROUP BY CONVERT(Date, CreatedTime)" +
                    $" ORDER BY Date DESC");
            }
        }

        public static dynamic GetTopBalance(int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select AccountID, Username, UserType, DisplayName, Gold, Coin, LockedGold, CreatedTime, Tel, LastActive, " +
                    $"IsBlocked, IsMute from [dbo].[Account] order by Gold desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetInfo(string accountName)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.QueryFirstOrDefault($"select AccountID, DisplayName from [dbo].[Account] where DisplayName LIKE '%{accountName}%'");
            }
        }

        public static dynamic GetInfoByUsername(string username)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.QueryFirstOrDefault($"select AccountID, DisplayName from [dbo].[Account] where Username LIKE '%{username}%'");
            }
        }

        public static List<TransferLog> GetTransferLog(long accountId, int skip, int itemPerpage)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_AccountID", accountId),
                    new SqlParameter("@_Skip", skip),
                    new SqlParameter("@_ItemPerPage", itemPerpage)
                };
                return helper.GetListSP<TransferLog>("SP_GetTransferLog", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<TransferLog>();
        }

        public static dynamic GetPlayLog(int gameId, long sessionId)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var query = $"SELECT * FROM (SELECT * FROM [log].[V_GameGoldTransaction] WITH (nolock) WHERE GameId = {gameId} AND Description LIKE '%{sessionId}%'";
                query += " UNION";
                query += $" SELECT * FROM [log].[GameGoldTransactionFull] WITH (nolock) WHERE GameId = {gameId} AND Description LIKE '%{sessionId}%') A";

                return connection.Query(query);
            }
        }

        public static List<VIPInfo> GetVIPInfo(long accountId)
        {
            DBHelper helper = new DBHelper(_conStr);
            return helper.GetListSP<VIPInfo>("SP_GetVIPInfo", new SqlParameter("@_AccountId", accountId));
        }
    }
}