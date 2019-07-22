using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Bussiness.ConfigCard;
using PortalManagement.Models.Bussiness.Payment;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.Payment
{
    public class PayDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static long AddLog(long accountId, string accountName, int type, long amount, string reason, int adder, string adderName)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                var response = connection.QueryFirst($"insert into dbo.Topup (Type, Value, Description, Adder, AdderName, AccountID, AccountName) " +
                    $"VALUES ({type}, {amount}, N'{reason}', {adder}, '{adderName}', {accountId}, '{accountName}')" +
                    $" select @@IDENTITY ID");

                return Convert.ToInt64(response.ID);
            }
        }

        public static void Update(string query)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute(query);
            }
        }

        //[dbo].[SP_Topup]
        //@_AccountId bigint,
        //@_ServiceId bigint,
        // @_Amount bigint,
        //    @_MoneyType int,
        // @_Description nvarchar(150),
        // @_ResponseStatus int output,
        //    @_Balance bigint output,
        //    @_Type  int,
        // @_ReferId bigint = null

        public static void AddGold(long accountId, long amount, string des, long referId, long serviceId, int moneyType, int type = 1)
        {
            DBHelper db = new DBHelper(_conStrPortalDbRoot);
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@_AccountId", accountId));
            pars.Add(new SqlParameter("@_ServiceId", serviceId));
            pars.Add(new SqlParameter("@_Amount", amount));
            pars.Add(new SqlParameter("@_Description", des));
            pars.Add(new SqlParameter("@_MoneyType", moneyType));
            pars.Add(new SqlParameter("@_ResponseStatus", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
            pars.Add(new SqlParameter("@_Balance", System.Data.SqlDbType.BigInt) { Direction = System.Data.ParameterDirection.Output });
            pars.Add(new SqlParameter("@_Type", type));
            pars.Add(new SqlParameter("@_ReferId", referId));

            db.ExecuteNonQuerySP("[dbo].[SP_Topup]", pars.ToArray());
        }

        public static dynamic Sumary(int start, int end)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                return connection.Query($"select sum(Value) Total, Type, CreatedTimeInt from dbo.Topup where CreatedTimeInt >= {start} and CreatedTimeInt <= {end} " +
                    $"group by Type, CreatedTimeInt " +
                    $"order by CreatedTimeInt desc");
            }
        }

        public static dynamic GetLog(int start, int end, int skip, int itemPerpage)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                return connection.Query($"select * from dbo.Topup where CreatedTimeInt >= {start} and CreatedTimeInt <= {end} " +
                    $"order by CreatedTimeInt desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetLog(int accountId)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                return connection.Query($"select top 50 * from dbo.Topup where Adder = {accountId} " +
                    $"order by CreatedTimeInt desc");
            }
        }

        public static List<UnverifyPayment> GetUnverifyPayment()
        {
            DBHelper db = new DBHelper(_conStr);
            return db.GetListSP<UnverifyPayment>("CMS_GetUnverifyPayment");
        }

        public static bool AcceptCard(long cardId)
        {
            DBHelper helper = null;
            try
            {
                helper = new DBHelper(_conStr);
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_CardID", cardId));
                parsList.Add(new SqlParameter("@_ResponseStatus", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });

                helper.ExecuteNonQuerySP("AcceptCard", parsList.ToArray());
                NLogManager.LogMessage(cardId.ToString());
                NLogManager.LogMessage(Convert.ToInt32(parsList[1].Value).ToString());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return true;
        }

        public static bool DeleteCard(long cardId)
        {
            DBHelper helper = null;
            try
            {
                helper = new DBHelper(_conStr);
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_CardID", cardId));

                helper.ExecuteNonQuerySP("DeleteCashoutCard", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return true;
        }

        public static dynamic GetCardInBank()
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {                
                return helper.GetListSP<CardInBank>("CardBankStatus");
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<CardInBank>();
        }

        public static bool DeleteCardInBank(long cardId)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_ID", cardId));

                helper.ExecuteNonQuerySP("DeleteCardFromCardBank", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return true;
        }

        public static List<ErrorCardTransaction> GetErrorCardTransaction()
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                return helper.GetListSP<ErrorCardTransaction>("GetPayError");
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<ErrorCardTransaction>();
        }

        public static bool InsertCard(string cardcode, string cardserial, long amount, string experied, int cardType, string telCode, string tradeMark, DateTime buyTime, string transactionCode, string resultCode, bool status)
        {
            DBHelper db = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_CardCode", cardcode),
                    new SqlParameter("@_CardSerial", cardserial),
                    new SqlParameter("@_Amount", amount),
                    new SqlParameter("@_Experied", experied),
                    new SqlParameter("@_CardType", cardType),
                    new SqlParameter("@_TelCode", telCode),
                    new SqlParameter("@_TradeMark", tradeMark),
                    new SqlParameter("@_BuyTime", buyTime),
                    new SqlParameter("@_TransactionCode", transactionCode),
                    new SqlParameter("@_ResultCode", resultCode),
                    new SqlParameter("@_Status", status)
                };

                SqlParameter[] _parsList = parsList.ToArray();
                db.ExecuteNonQuerySP("CMS_InputCard", _parsList);
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
            return false;
        }

        public static List<TopupBilling> GetTopupBilling(int from, int to)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_From", from),
                    new SqlParameter("@_To", to)
                };

                return helper.GetListSP<TopupBilling>("GetTopupBilling", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<TopupBilling>();
        }

        public static List<CashoutBilling> GetCashoutBilling(int from, int to)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_From", from),
                    new SqlParameter("@_To", to)
                };

                return helper.GetListSP<CashoutBilling>("GetCashoutBilling", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<CashoutBilling>();
        }

        public static List<PayLog> SearchCard(int searchType, string serial, string pin)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_SearchType", searchType),
                    new SqlParameter("@_Serial", serial),
                    new SqlParameter("@_Pin", pin)
                };
                return helper.GetListSP<PayLog>("SP_SearchCard", parsList.ToArray());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            finally
            {
                helper.Close();
            }
            return new List<PayLog>();
        }

        public static IEnumerable<CardConfig> GetCards()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query<CardConfig>("SELECT * FROM dbo.CardConfig");
            }
        }

        //Yagami Raito

        public static IEnumerable<PayLog> GetAllPayLogs()
        {
            using (var con = new SqlConnection(_conStrPortalDbRoot))
            {
                return con.Query<PayLog>("SELECT * FROM [dbo].[PayLogDetail]");
            }
        }

        public static IEnumerable<AgencyTransaction> GetAllAgencyTransactions()
        {
            using (var con= new SqlConnection(_conStrPortalDbRoot))
            {
                return con.Query<AgencyTransaction>("SELECT * FROM [GamePortal].[ag].[Transaction] WITH (NOLOCK)");
            }
        }

        public static IEnumerable<BalanceStatitic> GetAllBalanceStatitics()
        {
            using (var con = new SqlConnection(_conStrPortalDbRoot))
            {
                return con.Query<BalanceStatitic>("SELECT TOP 50 * FROM [GamePortal].[dbo].[BalanceStatistics] WITH(NOLOCK)");
            }
        }

        public static IEnumerable<Partner> GetAllPartners()
        {
            using (var con = new SqlConnection(_conStrPortalDbRoot))
            {
                return con.Query<Partner>("SELECT * FROM [GamePortal].[ag].[Account]");
            }
        }

       
    }
}