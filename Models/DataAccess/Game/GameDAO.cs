using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Game;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.Game
{
    public class GameDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static dynamic GetGame()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [game].[Game]");
            }
        }

        public static dynamic GetGameTransaction(long accountId, int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"SELECT * FROM (SELECT * FROM [log].[V_GameGoldTransaction] WITH (nolock) WHERE AccountID = {accountId}" +
                    $" UNION" +
                    $" SELECT * FROM [log].[GameGoldTransactionFull] WITH (nolock) WHERE AccountID = {accountId}) A" +
                    $" ORDER BY createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetBalanceHistory(long accountId, int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from [log].[V_TopupGold] with (nolock) where AccountID = {accountId} order by createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetTopupHistory(long accountId, int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from [log].[TopupCard] with (nolock) where AccountID = {accountId} order by createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static dynamic GetCashoutHistory(long accountId, int skip, int itemPerpage)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from [log].[CashoutCard] with (nolock) where AccountID = {accountId} order by createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static List<GameJackpot> GetJackpot(string start, string end)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                //NLogManager.LogMessage($"select * from [SlotMachine.Report].[dbo].[BigWinPlayers] where Type = 2 and CreatedDate >= '{start}' and CreatedDate <= '{end}' order by ID desc");
                return connection.Query<GameJackpot>($"select * from [SlotMachine.Report].[dbo].[BigWinPlayers] where Type = 2 and CreatedDate >= '{start}' and CreatedDate <= '{end}' order by ID desc").ToList();
            }
        }

        public static dynamic GetFundMinipoker()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [CardGame.MiniPokerDB].[dbo].[Funds] where BetType = 1");
            }
        }

        public static dynamic GetFundSlot1()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [eBankGame.SlotDiamondDB].[dbo].[RoomFunds]");
            }
        }

        public static dynamic GetFundSlot2()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [eBankGame.SlotIslandsDB].[dbo].[RoomFunds]");
            }
        }

        public static dynamic GetFundSlot3()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [Slot.25Lines].[dbo].[RoomFund]");
            }
        }

        public static dynamic GetFundThanQuay()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [eBankGame.SlotsGodDB].[dbo].[RoomFunds]");
            }
        }

        public static dynamic GetFundHilo()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [MiniGame.Hilo].[dbo].[Funds] where BetType = 1");
            }
        }

        public static dynamic GetFundSuperNova()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [MiniGame.SuperNova].[dbo].[RoomFunds]");
            }
        }

        public static dynamic GetFundHooHeyHow()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * from [HooHeyHow].[dbo].[GoldFund]");
            }
        }

        public static dynamic GetFundDiskShaking()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("select * FROM [CardGame].[dbo].[DiskShaking.GoldFund]");
            }
        }

        public static dynamic GetFundChangeHistory()
        {
            using (var connection = new SqlConnection(_conStr))
            {
                return connection.Query("select top 50 * from [dbo].[ChangeFundLog] order by id desc");
            }
        }

        public static IEnumerable<dynamic> ExecuteQueryWithResultRootDb(string query)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query(query);
            }
        }

        public static dynamic GetMinusGameFund()
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT TOP 1 Total FROM [GamePortal].[dbo].[MinusGameFund]");
            }
        }

        public static void AddEditFundLog(string accountName, string gameName, long amount, long fund, int roomId)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                connection.Execute($"insert into dbo.ChangeFundLog (AccountName, GameName, Amount, CreatedTime, Fund, RoomID) values " +
                    $"(N'{accountName}', N'{gameName}', {amount}, GETDATE(), {fund}, {roomId})");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="type">1: tru quy, 2: cong quy</param>
        /// <returns></returns>
        public static int UpdateMinusGameFund(long amount, int type)
        {
            DBHelper helper = new DBHelper(_conStr);
            try
            {
                List<SqlParameter> parsList = new List<SqlParameter>
                {
                    new SqlParameter("@_Amount", amount),
                    new SqlParameter("@_Type", type),
                    new SqlParameter("@_ResponseStatus", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output }
                };
                helper.ExecuteNonQuerySP("SP_UpdateMinusGameFund", parsList.ToArray());
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

        public static dynamic GetLuckySpinFund(int start, int end)
        {
            using (var connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT * FROM [LuckySpin].[dbo].[LuckySpin.Fund] WITH (NOLOCK) ORDER BY Date DESC");
            }
        }
    }
}