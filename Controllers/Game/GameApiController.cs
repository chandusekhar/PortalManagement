using PortalManagement.Financial;
using PortalManagement.Models;
using PortalManagement.Models.DataAccess.Game;
using PortalManagement.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Game
{
    public class GameApiController : ApiController
    {
        [HttpGet]
        public List<GameJackpot> GetListJackpot(string start, string end)
        {
            try
            {
                end += " 23:59:59 PM";
                var jp = GameDAO.GetJackpot(start, end);
                jp.ForEach(i =>
                {
                    i.GameName = SimpleCache.Game.FirstOrDefault(x => x.Key == i.GameID).Value;
                });

                return jp;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public bool ModifyFund(int gameId, int roomId, long value1)
        {
            try
            {
                string gameName = string.Empty;
                string query = string.Empty;

                switch (gameId)
                {
                    case 1:
                        gameName = "Nông trại";
                        query += "declare @_fund bigint\n";
                        query += $"update [eBankGame.SlotDiamondDB].[dbo].[RoomFunds] set @_fund = PrizeFund -= {value1} where RoomID = {roomId}";
                        break;
                    case 2:
                        gameName = "Mafia";
                        query += "declare @_fund bigint\n";
                        query += $"update [eBankGame.SlotIslandsDB].[dbo].[RoomFunds] set @_fund = PrizeFund -= {value1} where RoomID = {roomId}";
                        break;
                    case 3:
                        gameName = "Hải vương";
                        query += "declare @_fund int\n";
                        query += $"update [Slot.25Lines].[dbo].[RoomFund] set @_fund = PrizeFund -= {value1} where RoomID = {roomId}";
                        break;
                    case 4:
                        gameName = "Vua bão";
                        query += "declare @_fund bigint\n";
                        query += $"update [eBankGame.SlotsGodDB].[dbo].[RoomFunds] set @_fund = PrizeFund -= {value1} where RoomID = {roomId}";
                        break;
                    case 6:
                        gameName = "Minipoker";
                        query += "declare @_fund bigint\n";
                        query += $"update [CardGame.MiniPokerDB].[dbo].[Funds] set @_fund = PrizeFund -= {value1} where RoomID = {roomId} and BetType = 1";
                        break;
                    case 7:
                        gameName = "Cao thấp";
                        query += "declare @_fund bigint\n";
                        query += $"update [MiniGame.Hilo].[dbo].[Funds] set @_fund = PrizeFund -= {value1} where RoomID = {roomId} and BetType = 1";
                        break;
                    //case 8:
                    //    gameName = "Bầu cua";
                    //    query += "declare @_fund bigint\n";
                    //    query += $"update [HooHeyHow].[dbo].[GoldFund] set @_fund = Fund -= {value1}";
                    //    break;
                    //case 9:
                    //    gameName = "Xóc xóc";
                    //    query += "declare @_fund bigint\n";
                    //    query += $"update [CardGame].[dbo].[DiskShaking.GoldFund] set @_fund = Fund -= {value1}";
                    //    break;
                }
                query += "\nselect @_fund response";
                var newFund = Convert.ToInt64(GameDAO.ExecuteQueryWithResultRootDb(query).FirstOrDefault().response);
                GameDAO.AddEditFundLog(UserContext.UserInfo.FullName, gameName, value1, newFund, roomId);
                GameDAO.UpdateMinusGameFund(value1, 2);
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }

        [HttpGet]
        public dynamic GetMinusGameFund()
        {
            try
            {
                return GameDAO.GetMinusGameFund();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return 0;
        }

        [HttpGet]
        public dynamic GetFundChangeHistory()
        {
            try
            {
                return GameDAO.GetFundChangeHistory();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }
        [HttpGet]
        public async Task<List<Fee>> GetListFee(int start, int end)
        {
            try
            {
                List<Fee> fees = new List<Fee>();
                using (var financial = new FinancialSoapClient())
                {
                    var response = await financial.GetFeeByRangeAsync(start, end);
                    var data = response.Body.GetFeeByRangeResult;
                    foreach (var i in data)
                    {
                        fees.Add(new Fee
                        {
                            Day = i.Day,
                            Fees = i.Fees.ToDictionary(x => x.GameId, x => x.Fee)
                        });
                    }
                }
                fees.Reverse();

                return fees;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic GetLuckySpinFund(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");

                return GameDAO.GetLuckySpinFund(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }
    }

    public class Fee
    {
        public string Day { get; set; }
        public Dictionary<int, long> Fees { get; set; }
    }
}
