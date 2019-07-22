using PortalManagement.Models;
using PortalManagement.Models.DataAccess.Game;
using PortalManagement.Models.DataAccess.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.PlayHistory
{
    public class PlayHistoryApiController : ApiController
    {
        [HttpGet]
        public List<dynamic> GetGameHistory(int searchType, long? accountId, string username, string displayName, int page = 0)
        {
            try
            {
                var gameLst = SimpleCache.Game;
                var skipItem = 20 * page;

                long id = 0;
                if (searchType == 0) id = accountId.Value;
                else if (searchType == 1)
                {
                    var acc = UserDAO.GetInfo(displayName);
                    if (acc != null) id = acc.AccountID;
                }
                else if (searchType == 2)
                {
                    var acc = UserDAO.GetInfoByUsername(username);
                    if (acc != null) id = acc.AccountID;
                }

                var transaction = GameDAO.GetGameTransaction(id, skipItem, 20);
                List<dynamic> data = new List<dynamic>();                
                foreach (var trans in transaction)
                {
                    data.Add(new
                    {
                        AccountId = id,
                        Game = gameLst[trans.GameId],
                        CreatedTime = trans.CreatedTime,
                        Amount = trans.Amount,
                        Balance = trans.Balance,
                        Description = trans.Description,
                        Type = trans.Type
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return new List<dynamic>();
        }
    }
}
