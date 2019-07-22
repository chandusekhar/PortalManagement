using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PortalManagement.Models.Bussiness.AddFeatures;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.AddFeatures
{
    public static class AddFeaturesDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        

        public static int GetJackpotPrize(int createdId, string createdName, string displayName, string ip, int gameId, int roomId)
        {
         
            try
            {
                var db = new DBHelper(_conStr);

                var pars = new SqlParameter[8];
                pars[0] = new SqlParameter("@_CreatedID", createdId);
                pars[1] = new SqlParameter("@_CreatedName", createdName);
                pars[2] = new SqlParameter("@_DisplayName", displayName);
                pars[3] = new SqlParameter("@_IPAdress", ip);
                pars[4] = new SqlParameter("@_GameID", gameId);
                pars[5] = new SqlParameter("@_RoomID", roomId);
                pars[6] = new SqlParameter("@_JackpotPrize",SqlDbType.BigInt) {Direction = ParameterDirection.Output};
                pars[7] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) {Direction = ParameterDirection.Output};

                db.ExecuteNonQuerySP("SP_CMS_GetJackpotPrize", pars.ToArray());

                int.TryParse(pars[7].Value.ToString(), out var response);
                NLogManager.LogMessage($"GetJackpotPrize => CreatedID:{createdId}|CreatedName:{createdName}|DisplayName:{displayName}|IP:{ip}|GameID:{gameId}|RoomID:{roomId}|JackpotPrize:{pars[6].Value}|Response:{pars[7].Value}");
                return response;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }


            return -99;
        }

        public static IEnumerable<JackpotPrizeLog> GetLog()
        {
            try
            {
                var db = new DBHelper(_conStr);
                return db.GetListSP<JackpotPrizeLog>("SP_CMS_GetJackpotPrizeLog");

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
                return null;
            }
        }
    }

}