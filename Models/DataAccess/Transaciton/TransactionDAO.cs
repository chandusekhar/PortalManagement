using Dapper;
using PortalManagement.Models.Bussiness.Agency;
using PortalManagement.Models.Bussiness.Transaction;
using PortalManagement.Models.Utilities.ConnectionString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.Transaction
{
    public class TransactionDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static int RefundTransaction(long tranId, int creatorId, string creator, string reason)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_Id", tranId),
                    new SqlParameter("@_Creator", creator),
                    new SqlParameter("@_CreatorId", creatorId),
                    new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                helper.ExecuteNonQuerySP("[dbo].[SP_RefundTransaction]", parsList.ToArray());
                return Convert.ToInt32(parsList[3].Value);
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

        public static dynamic Search(long id)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.QueryFirstOrDefault($"SELECT * FROM [GamePortal].[log].[TransferTransaction] with (nolock) where ID = {id}" +
                    $" AND ID NOT IN (SELECT Id FROM [GameContentManagementSystem].[dbo].[RefundLog])");
            }
        }

        public static dynamic GetRefundLog(int curLevel, string creator)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                var query = $"SELECT * FROM [log].[RefundLog] with (nolock)";
                if (curLevel != 0)
                    query += $" where Creator = {creator}";

                return connection.Query(query);
            }
        }

        public static dynamic GetTransactionFees(DateTime start, DateTime end)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var query = $"SELECT CONVERT(Date, CreatedTime) Date, SUM(SendAmount - RecvAmount) TotalFee FROM [log].[TransferTransaction] with (nolock)" +
                    $" WHERE CreatedTime >= '{start}' AND CreatedTime <= '{end}'" +
                    $" GROUP BY CONVERT(Date, CreatedTime) ORDER BY Date DESC";

                return connection.Query(query);
            }
        }

        public static dynamic GetTransactionByDate(string date, int skip, int itemPerPage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var query = $"SELECT SendName, RecvName, (SendAmount - RecvAmount) Fee, CreatedTime FROM [log].[TransferTransaction] with (nolock)" +
                    $" WHERE CONVERT(Date, CreatedTime ) = '{date}' ORDER BY CreatedTime DESC offset {skip} rows fetch next {itemPerPage} rows only";

                return connection.Query(query);
            }
        }

        public static dynamic GetTransactionLog(DateTime start, DateTime end)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var query = $"SELECT CONVERT(Date, CreatedTime) Date, SUM(SendAmount - RecvAmount) TotalFee FROM [log].[TransferTransaction] with (nolock)" +
                    $" WHERE CreatedTime >= '{start}' AND CreatedTime <= '{end}'" +
                    $" GROUP BY CONVERT(Date, CreatedTime) ORDER BY Date DESC";

                return connection.Query(query);
            }
        }

        public static List<AgencyTransaction> TransactionHistory(long id, DateTime start, DateTime end, int type, int skip, int itemPerPage, out long totalMoney, out long totalFee)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_GameAccountId", id),
                    new SqlParameter("@_Start", start),
                    new SqlParameter("@_End", end),
                    new SqlParameter("@_Type", type),
                    new SqlParameter("@_Skip", skip),
                    new SqlParameter("@_ItemPerPage", itemPerPage),
                    new SqlParameter("@_TotalMoney", SqlDbType.BigInt) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_TotalFee", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
                };
                totalMoney = Convert.ToInt64(parsList[6].Value);
                NLogManager.LogMessage(totalMoney.ToString());
                totalFee = Convert.ToInt64(parsList[7].Value);
                return helper.GetListSP<AgencyTransaction>("[dbo].[GetAgencyTransaction]", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            totalMoney = 0;
            totalFee = 0;
            return new List<AgencyTransaction>();
        }

        public static dynamic GetAgencyInfo(long accountId)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.QueryFirstOrDefault($"select * from ag.Account where GameAccountId = {accountId}");
            }
        }

        public static void UpdateState(long transId, int state, long accountId)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update ag.[Transaction] set State = {state} where Id = {transId} and {accountId} in (SenderID, RecvID)");
            }
        }

        public static void UpdateStateAdmin(long transId, int state)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute($"update ag.[Transaction] set State = {state} where Id = {transId}");
            }
        }

        public static List<RefundLog> GetRefundLogs(long? accountId, int from, int to)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            var query = "";
            if (!accountId.HasValue)
                query = $"select * from ag.RefundLog where CreatedTimeInt >= {from} and CreatedTimeInt <= {to}";
            else
                query = $"select * from ag.RefundLog where CreatedTimeInt >= {from} and CreatedTimeInt <= {to} and AccountId = {accountId}";

            return helper.GetList<RefundLog>(query);
        }

        public static List<TransactionLog> GetTransactionLogs(long? accountId, int from, int to)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            var query = "";
            if (!accountId.HasValue)
                query = $"select * from [GamePortal].[ag].[Transaction] where CreatedTimeInt >= {from} and CreatedTimeInt <= {to} AND Fee > 0";
            else
                query = $"select * from [GamePortal].[ag].[Transaction] where CreatedTimeInt >= {from} and CreatedTimeInt <= {to} and {accountId.Value} IN (SenderID, RecvID) AND Fee > 0";

            return helper.GetList<TransactionLog>(query);
        }

        public static List<TotalAgencyTransaction> GetAgencyTotalTransaction(long gameAccountId, int start, int end)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot);
            var query = $"SELECT CreatedTimeInt CreatedDateInt, SUM(Amount) TotalTransaction FROM [GamePortal].[ag].[Transaction] WITH (NOLOCK)" +
                        $" WHERE CreatedTimeInt >= {start} AND CreatedTimeInt <= {end}" +
                        $" AND ((SenderID = {gameAccountId} AND RecvID NOT IN (SELECT GameAccountId From [ag].[Account] WITH (NOLOCK))) OR (RecvID = {gameAccountId} AND SenderID NOT IN (SELECT GameAccountId From [ag].[Account] WITH (NOLOCK))))" +
                        $" GROUP BY CreatedTimeInt";
            return helper.GetList<TotalAgencyTransaction>(query);
        }

        public static List<AgencyTransAnalytic> GetAgencyTransactionsAnalytic(int start, int end)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_From", start),
                    new SqlParameter("@_To", end)
                };
                return helper.GetListSP<AgencyTransAnalytic>("CMS_AgencyTransactionsAnalytic", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<AgencyTransAnalytic>();
        }
    }
}