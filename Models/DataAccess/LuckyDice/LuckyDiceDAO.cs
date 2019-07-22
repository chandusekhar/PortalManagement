using PortalManagement.Models.LuckyDice;
using PortalManagement.Models.Utilities.ConnectionString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.LuckyDice
{
    public static class LuckyDiceDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static BotTaxiConfig GetConfigBotTaxi()
        {
            DBHelper helper = null;
            try
            {
                helper = new DBHelper(_conStr);

                return helper.GetInstanceSP<BotTaxiConfig>("LuckyDice_GetBotConfig");
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

        public static long BotTaxiUpdate(BotTaxiConfig newConfig)
        {
            try
            {
                var db = new DBHelper(_conStr);
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_MinBot", newConfig.MinBot));
                parsList.Add(new SqlParameter("@_MaxBot", newConfig.MaxBot));
                parsList.Add(new SqlParameter("@_NumRichBot", newConfig.NumRichBot));
                parsList.Add(new SqlParameter("@_NumNormalBot", newConfig.NumNormalBot));
                parsList.Add(new SqlParameter("@_NumPoorBot", newConfig.NumPoorBot));
                parsList.Add(new SqlParameter("@_VipChangeRate", newConfig.VipChangeRate));
                parsList.Add(new SqlParameter("@_NorChangeRate", newConfig.NorChangeRate));
                parsList.Add(new SqlParameter("@_PoorChangeRate", newConfig.PoorChangeRate));
                parsList.Add(new SqlParameter("@_MinTimeChange", newConfig.MinTimeChange));
                parsList.Add(new SqlParameter("@_MaxTimeChange", newConfig.MaxTimeChange));
                parsList.Add(new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output });

                db.ExecuteNonQuerySP("LuckyDice_UpdateBotConfig", parsList.ToArray());
                return long.Parse(parsList[10].Value.ToString());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
                return -199;
            }
        }

        public static FundInfo GetFundTaxi()
        {
            DBHelper helper = null;
            try
            {

                helper = new DBHelper(_conStr);
                var info = new FundInfo();
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_BotProfit", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });

                info.Config = helper.GetInstanceSP<FundConfig>("[dbo].[LuckyDice_GetFundInfo]", parsList.ToArray());
                info.BotProfit = int.Parse(parsList[0].Value.ToString());
                return info;
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

        public static IEnumerable<BotBetData> GetBetData()
        {
            try
            {

                var helper = new DBHelper(_conStr);

                return helper.GetListSP<BotBetData>("LuckyDice_GetBetData");

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        public static int UpdateBetData(int betValue, int vip, int quantity)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_BetValue", betValue);
                pars[1] = new SqlParameter("@_Vip", vip);
                pars[2] = new SqlParameter("@_Quantity", quantity);
                pars[3] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var db = new DBHelper(_conStr);
                db.ExecuteNonQuerySP("LuckyDice_UpdateBetData", pars);

                return int.Parse(pars[3].Value.ToString());
            }
            catch (Exception e)
            {
                NLogManager.PublishException(e);
                return -199;
            }
        }
    }
}