using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.OtherFeatures
{
    public static class OtherFeaturesDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
       

        public static int SetJackpotPrize(int gameId, int roomId)
        {
            try
            {
                var db = new DBHelper(_conStr);
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_GameId", gameId);
                pars[1] = new SqlParameter("@_RoomId", roomId);
                pars[2] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db.ExecuteNonQuerySP("SP_SlotMachine_SetJackpotPrize", pars);

                int.TryParse(pars[2].Value.ToString(), out var response);

                NLogManager.LogMessage($"SetJackpotPrize: GameId:{gameId}|roomId:{roomId}");

                return response;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
                return -101;
            }
        }
    }
}